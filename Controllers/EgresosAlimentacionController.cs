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
    public class EgresosAlimentacionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EgresosAlimentacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EgresosAlimentacion
        public async Task<IActionResult> Index()
        {
            var egresosA = _context.EgresoAlimentaciones.Include(e => e.Planta);
            return View(await egresosA.ToListAsync());
        }

        // GET: EgresosAlimentacion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var egresoA = await _context.EgresoAlimentaciones
                .Include(e => e.Planta)
                .FirstOrDefaultAsync(m => m.IdEgresoA == id);
            if (egresoA == null)
            {
                return NotFound();
            }
            return View(egresoA);
        }

        // GET: EgresosAlimentacion/Create
        public IActionResult Create()
        {
            ViewData["IdPlanta"] = new SelectList(_context.Plantas, "IdPlanta", "NPlanta");
            return View();
        }

        // POST: EgresosAlimentacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEgresoA,IdPlanta,Fecha,Responsable,Producto")] EgresoAlimentacion egresoA)
        {
            Console.WriteLine($"[LOG] POST Create EgresoAlimentacion ejecutado. Producto: {egresoA?.Producto}");
            if (ModelState.IsValid)
            {
                Console.WriteLine("[LOG] ModelState válido. Se intenta agregar el egresoA...");
                _context.EgresoAlimentaciones.Add(egresoA);
                await _context.SaveChangesAsync();
                Console.WriteLine($"[LOG] EgresoAlimentacion creado con IdPlanta: {egresoA.IdPlanta}");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Console.WriteLine("[LOG] ModelState inválido. No se crea el egresoA.");
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"[ERROR] Campo: {key}, Error: {error.ErrorMessage}");
                    }
                }
            }
            ViewData["IdPlanta"] = new SelectList(_context.Plantas, "IdPlanta", "NPlanta", egresoA.IdPlanta);
            return View(egresoA);
        }
        

        // GET: EgresosAlimentacion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var egresoA = await _context.EgresoAlimentaciones.FindAsync(id);
            if (egresoA == null)
            {
                return NotFound();
            }
            ViewData["IdPlanta"] = new SelectList(_context.Plantas, "IdPlanta", "NPlanta", egresoA.IdPlanta);
            return View(egresoA);
        }

        // POST: EgresosAlimentacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEgresoA,IdPlanta,Fecha,Responsable,Producto")] EgresoAlimentacion egresoA)
        {
            if (id != egresoA.IdEgresoA)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.EgresoAlimentaciones.Update(egresoA);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EgresoAExists(egresoA.IdEgresoA))
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
            ViewData["IdPlanta"] = new SelectList(_context.Plantas, "IdPlanta", "NPlanta", egresoA.IdPlanta);
            return View(egresoA);
        }

        // GET: EgresosAlimentacion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var egresoA = await _context.EgresoAlimentaciones
                .Include(e => e.Planta)
                .FirstOrDefaultAsync(m => m.IdEgresoA == id);
            if (egresoA == null)
            {
                return NotFound();
            }
            return View(egresoA);
        }

        // POST: EgresosAlimentacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var egresoA = await _context.EgresoAlimentaciones.FindAsync(id);
            if (egresoA != null)
            {
                _context.EgresoAlimentaciones.Remove(egresoA);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EgresoAExists(int id)
        {
            return _context.EgresoAlimentaciones.Any(e => e.IdEgresoA == id);
        }
    }
}
