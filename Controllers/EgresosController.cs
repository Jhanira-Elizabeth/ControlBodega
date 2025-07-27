using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControlBodega.Data;
using ControlBodega.Models;

namespace ControlBodega.Controllers
{
    public class EgresosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EgresosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Egresos
        public async Task<IActionResult> Index()
        {
            var egresos = _context.Egresos.Include(e => e.Planta);
            return View(await egresos.ToListAsync());
        }

        // GET: Egresos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var egreso = await _context.Egresos
                .Include(e => e.Planta)
                .FirstOrDefaultAsync(m => m.IdEgreso == id);
            if (egreso == null)
            {
                return NotFound();
            }

            return View(egreso);
        }

        // GET: Egresos/Create
        public IActionResult Create()
        {
            ViewData["IdPlanta"] = new SelectList(_context.Plantas, "IdPlanta", "NPlanta");
            return View();
        }

        // POST: Egresos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPlanta,Fecha,Producto,Medida,Cantidad,Responsable,Tipo")] Egreso egreso)
        {
            Console.WriteLine($"[LOG] POST Create Egreso ejecutado. Producto: {egreso?.Producto}");
            if (ModelState.IsValid)
            {
                Console.WriteLine("[LOG] ModelState válido. Se intenta agregar el egreso...");
                _context.Add(egreso);
                await _context.SaveChangesAsync();
                Console.WriteLine($"[LOG] Egreso creado con IdPlanta: {egreso.IdPlanta}");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Console.WriteLine("[LOG] ModelState inválido. No se crea el egreso.");
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"[ERROR] Campo: {key}, Error: {error.ErrorMessage}");
                    }
                }
            }
            ViewData["IdPlanta"] = new SelectList(_context.Plantas, "IdPlanta", "NPlanta", egreso.IdPlanta);
            return View(egreso);
        }
        

        // GET: Egresos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var egreso = await _context.Egresos.FindAsync(id);
            if (egreso == null)
            {
                return NotFound();
            }
            ViewData["IdPlanta"] = new SelectList(_context.Plantas, "IdPlanta", "NPlanta", egreso.IdPlanta);
            return View(egreso);
        }

        // POST: Egresos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEgreso,IdPlanta,Fecha,Producto,Medida,Cantidad,Responsable,Tipo")] Egreso egreso)
        {
            if (id != egreso.IdEgreso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(egreso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EgresoExists(egreso.IdEgreso))
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
            ViewData["IdPlanta"] = new SelectList(_context.Plantas, "IdPlanta", "NPlanta", egreso.IdPlanta);
            return View(egreso);
        }

        // GET: Egresos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var egreso = await _context.Egresos
                .Include(e => e.Planta)
                .FirstOrDefaultAsync(m => m.IdEgreso == id);
            if (egreso == null)
            {
                return NotFound();
            }

            return View(egreso);
        }

        // POST: Egresos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var egreso = await _context.Egresos.FindAsync(id);
            if (egreso != null)
            {
                _context.Egresos.Remove(egreso);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EgresoExists(int id)
        {
            return _context.Egresos.Any(e => e.IdEgreso == id);
        }
    }
}
