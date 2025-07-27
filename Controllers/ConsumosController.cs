using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControlBodega.Data;
using ControlBodega.Models;

namespace ControlBodega.Controllers
{
    public class ConsumosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConsumosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Consumos
        public async Task<IActionResult> Index()
        {
            var consumos = _context.Consumos.Include(c => c.Egreso);
            return View(await consumos.ToListAsync());
        }

        // GET: Consumos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var consumo = await _context.Consumos
                .Include(c => c.Egreso)
                .FirstOrDefaultAsync(m => m.IdConsumo == id);
            if (consumo == null)
            {
                return NotFound();
            }
            return View(consumo);
        }

        // GET: Consumos/Create
        public IActionResult Create()
        {
            ViewData["IdEgreso"] = new SelectList(_context.Egresos, "IdEgreso", "Producto");
            return View();
        }

        // POST: Consumos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdConsumo,IdEgreso,Producto,Medida,Fecha,Hora,Tanque,Poblacion,Estadio")] Consumo consumo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(consumo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEgreso"] = new SelectList(_context.Egresos, "IdEgreso", "Producto", consumo.IdEgreso);
            return View(consumo);
        }

        // GET: Consumos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var consumo = await _context.Consumos.FindAsync(id);
            if (consumo == null)
            {
                return NotFound();
            }
            ViewData["IdEgreso"] = new SelectList(_context.Egresos, "IdEgreso", "Producto", consumo.IdEgreso);
            return View(consumo);
        }

        // POST: Consumos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdConsumo,IdEgreso,Producto,Medida,Fecha,Hora,Tanque,Poblacion,Estadio")] Consumo consumo)
        {
            if (id != consumo.IdConsumo)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consumo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsumoExists(consumo.IdConsumo))
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
            ViewData["IdEgreso"] = new SelectList(_context.Egresos, "IdEgreso", "Producto", consumo.IdEgreso);
            return View(consumo);
        }

        // GET: Consumos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var consumo = await _context.Consumos
                .Include(c => c.Egreso)
                .FirstOrDefaultAsync(m => m.IdConsumo == id);
            if (consumo == null)
            {
                return NotFound();
            }
            return View(consumo);
        }

        // POST: Consumos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consumo = await _context.Consumos.FindAsync(id);
            if (consumo != null)
            {
                _context.Consumos.Remove(consumo);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsumoExists(int id)
        {
            return _context.Consumos.Any(e => e.IdConsumo == id);
        }
    }
}
