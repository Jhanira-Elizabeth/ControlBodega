using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControlBodega.Data;
using ControlBodega.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ControlBodega.Controllers
{
    public class ResumenEgresosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResumenEgresosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Planta = new SelectList(_context.Plantas, "IdPlanta", "NPlanta");
            var model = new ResumenEgresoViewModel();
            return View(model);
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Index(int? IdPlanta, DateTime? FechaInicio, DateTime? FechaFin)
        {
            var plantas = _context.Plantas.ToList();
            ViewBag.Planta = new SelectList(plantas, "IdPlanta", "NPlanta", IdPlanta);
            var egresos = await _context.Egresos
                .Where(e => (!IdPlanta.HasValue || e.IdPlanta == IdPlanta) && (!FechaInicio.HasValue || e.Fecha >= FechaInicio) && (!FechaFin.HasValue || e.Fecha <= FechaFin))
                .ToListAsync();
            var egresosAlimentacion = await _context.EgresoAlimentaciones
                .Where(ea => (!IdPlanta.HasValue || ea.IdPlanta == IdPlanta) && (!FechaInicio.HasValue || ea.Fecha >= FechaInicio) && (!FechaFin.HasValue || ea.Fecha <= FechaFin))
                .ToListAsync();
            var consumos = await _context.Consumos
                .Where(c => (!IdPlanta.HasValue || c.IdPlanta == IdPlanta) && (!FechaInicio.HasValue || c.Fecha >= FechaInicio) && (!FechaFin.HasValue || c.Fecha <= FechaFin))
                .ToListAsync();
            var model = new ResumenEgresoViewModel
            {
                IdPlanta = IdPlanta,
                NombrePlanta = plantas.FirstOrDefault(p => p.IdPlanta == IdPlanta)?.NPlanta,
                FechaInicio = FechaInicio,
                FechaFin = FechaFin,
                Egresos = egresos,
                EgresosAlimentacion = egresosAlimentacion,
                Consumos = consumos
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ExportExcel(int IdPlanta, int Mes, int Anio)
        {
            var planta = await _context.Plantas.FindAsync(IdPlanta);
            var fechaInicio = new DateTime(Anio, Mes, 1);
            var fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
            var egresos = await _context.Egresos
                .Where(e => e.IdPlanta == IdPlanta && e.Fecha >= fechaInicio && e.Fecha <= fechaFin)
                .Select(e => new { e.Fecha, e.Producto, e.Cantidad })
                .ToListAsync();

            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Egresos");
                worksheet.Cell(1, 1).Value = "Fecha";
                worksheet.Cell(1, 2).Value = "Producto";
                worksheet.Cell(1, 3).Value = "Cantidad";
                int row = 2;
                foreach (var item in egresos)
                {
                    worksheet.Cell(row, 1).Value = item.Fecha.ToShortDateString();
                    worksheet.Cell(row, 2).Value = item.Producto;
                    worksheet.Cell(row, 3).Value = item.Cantidad;
                    row++;
                }
                using (var stream = new System.IO.MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ResumenEgresos_{planta?.NPlanta}_{Mes}_{Anio}.xlsx");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportPdf(int IdPlanta, int Mes, int Anio)
        {
            var planta = await _context.Plantas.FindAsync(IdPlanta);
            var fechaInicio = new DateTime(Anio, Mes, 1);
            var fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
            var egresos = await _context.Egresos
                .Where(e => e.IdPlanta == IdPlanta && e.Fecha >= fechaInicio && e.Fecha <= fechaFin)
                .Select(e => new { e.Fecha, e.Producto, e.Cantidad })
                .ToListAsync();

            var html = "<h2>Resumen de Egresos</h2>" +
                $"<h4>{planta?.NPlanta} - {System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mes)} {Anio}</h4>" +
                "<table border='1' cellpadding='5' cellspacing='0'><tr><th>Fecha</th><th>Producto</th><th>Cantidad</th></tr>" +
                string.Join("", egresos.Select(e => $"<tr><td>{e.Fecha.ToShortDateString()}</td><td>{e.Producto}</td><td>{e.Cantidad}</td></tr>")) +
                "</table>";

            using (var ms = new System.IO.MemoryStream())
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(30);
                        page.Header().Text($"Resumen de Egresos - {planta?.NPlanta} - {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mes)} {Anio}").FontSize(18).Bold().AlignCenter();
                        page.Content().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(100); // Fecha
                                columns.RelativeColumn(2);   // Producto
                                columns.ConstantColumn(80);  // Cantidad
                            });
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Fecha").Bold();
                                header.Cell().Element(CellStyle).Text("Producto").Bold();
                                header.Cell().Element(CellStyle).Text("Cantidad").Bold();
                            });
                            foreach (var item in egresos)
                            {
                                table.Cell().Element(CellStyle).Text(item.Fecha.ToShortDateString());
                                table.Cell().Element(CellStyle).Text(item.Producto);
                                table.Cell().Element(CellStyle).Text(item.Cantidad.ToString());
                            }
                        });
                        page.Footer().AlignCenter().Text($"Exportado el {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(10);
                    });
                }).GeneratePdf(ms);
                return File(ms.ToArray(), "application/pdf", $"ResumenEgresos_{planta?.NPlanta}_{Mes}_{Anio}.pdf");
            }

            IContainer CellStyle(IContainer container) => container.PaddingVertical(5).PaddingHorizontal(3).BorderBottom(1).BorderColor("#DDD");
        }
    }
}
