using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControlBodega.Data;
using ControlBodega.Models;

namespace ControlBodega.Controllers
{
    public class DevolucionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DevolucionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Devoluciones
        public async Task<IActionResult> Index()
        {
            var devoluciones = _context.Devoluciones.Include(d => d.Egreso).Include(d => d.Planta);
            return View(await devoluciones.ToListAsync());
        }

        // GET: Devoluciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devoluciones
                .Include(d => d.Egreso)
                .Include(d => d.Planta)
                .FirstOrDefaultAsync(m => m.IdDevolucion == id);
            if (devolucion == null)
            {
                return NotFound();
            }

            return View(devolucion);
        }

        // GET: Devoluciones/Create
        public IActionResult Create()
        {
            ViewData["IdPlanta"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Plantas, "IdPlanta", "NPlanta");
            return View();
        }

        // POST: Devoluciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDevolucion,IdEgreso,Producto,IdPlanta,Cantidad,FechaDevolucion,Motivo")] Devolucion devolucion)
        {
            Console.WriteLine($"[LOG] POST Create Devolucion ejecutado. Producto: {devolucion?.Producto}");
            if (!_context.Egresos.Any(e => e.IdEgreso == devolucion.IdEgreso))
            {
                ModelState.AddModelError("IdEgreso", "El ID de Egreso no existe.");
            }
            if (!_context.Plantas.Any(p => p.IdPlanta == devolucion.IdPlanta))
            {
                ModelState.AddModelError("IdPlanta", "La planta seleccionada no existe.");
            }
            if (ModelState.IsValid)
            {
                Console.WriteLine("[LOG] ModelState válido. Se intenta agregar la devolución...");
                _context.Add(devolucion);
                await _context.SaveChangesAsync();
                Console.WriteLine($"[LOG] Devolución creada con IdPlanta: {devolucion.IdPlanta}");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Console.WriteLine("[LOG] ModelState inválido. No se crea la devolución.");
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"[ERROR] Campo: {key}, Error: {error.ErrorMessage}");
                    }
                }
            }
            ViewData["IdPlanta"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Plantas, "IdPlanta", "NPlanta", devolucion.IdPlanta);
            return View(devolucion);
        }
        

        // GET: Devoluciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devoluciones.FindAsync(id);
            if (devolucion == null)
            {
                return NotFound();
            }
            ViewData["IdPlanta"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Plantas, "IdPlanta", "NPlanta", devolucion.IdPlanta);
            return View(devolucion);
        }

        // POST: Devoluciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDevolucion,IdEgreso,Producto,IdPlanta,Cantidad,FechaDevolucion,Motivo")] Devolucion devolucion)
        {
            if (id != devolucion.IdDevolucion)
            {
                return NotFound();
            }
            if (!_context.Egresos.Any(e => e.IdEgreso == devolucion.IdEgreso))
            {
                ModelState.AddModelError("IdEgreso", "El ID de Egreso no existe.");
            }
            if (!_context.Plantas.Any(p => p.IdPlanta == devolucion.IdPlanta))
            {
                ModelState.AddModelError("IdPlanta", "La planta seleccionada no existe.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(devolucion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DevolucionExists(devolucion.IdDevolucion))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPlanta"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Plantas, "IdPlanta", "NPlanta", devolucion.IdPlanta);
            return View(devolucion);
        }

        // GET: Devoluciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devoluciones
                .Include(d => d.Egreso)
                .Include(d => d.Planta)
                .FirstOrDefaultAsync(m => m.IdDevolucion == id);
            if (devolucion == null)
            {
                return NotFound();
            }

            return View(devolucion);
        }

        // POST: Devoluciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var devolucion = await _context.Devoluciones.FindAsync(id);
            if (devolucion != null)
            {
                _context.Devoluciones.Remove(devolucion);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DevolucionExists(int id)
        {
            return _context.Devoluciones.Any(e => e.IdDevolucion == id);
        }
    }
}
