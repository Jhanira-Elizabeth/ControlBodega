using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControlBodega.Data;
using ControlBodega.Models;

namespace ControlBodega.Controllers
{
    public class IngresosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IngresosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ingresos
        public async Task<IActionResult> Index()
        {
            var ingresos = _context.Ingresos.Include(i => i.Planta);
            return View(await ingresos.ToListAsync());
        }

        // GET: Ingresos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos
                .Include(i => i.Planta)
                .FirstOrDefaultAsync(m => m.IdIngreso == id);
            if (ingreso == null)
            {
                return NotFound();
            }

            return View(ingreso);
        }

        // GET: Ingresos/Create
        public IActionResult Create()
        {
            ViewData["IdPlanta"] = new SelectList(_context.Plantas, "IdPlanta", "NPlanta");
            return View();
        }

        // POST: Ingresos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPlanta,Producto,Medida,Cantidad,Fecha")] Ingreso ingreso)
        {
            Console.WriteLine($"[LOG] POST Create Ingreso ejecutado. Producto: {ingreso?.Producto}");
            if (ModelState.IsValid)
            {
                Console.WriteLine("[LOG] ModelState válido. Se intenta agregar el ingreso...");
                _context.Add(ingreso);
                await _context.SaveChangesAsync();
                Console.WriteLine($"[LOG] Ingreso creado con IdPlanta: {ingreso.IdPlanta}");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Console.WriteLine("[LOG] ModelState inválido. No se crea el ingreso.");
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"[ERROR] Campo: {key}, Error: {error.ErrorMessage}");
                    }
                }
            }
            ViewData["IdPlanta"] = new SelectList(_context.Plantas, "IdPlanta", "NPlanta", ingreso.IdPlanta);
            return View(ingreso);
        }
        

        // GET: Ingresos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos.FindAsync(id);
            if (ingreso == null)
            {
                return NotFound();
            }
            ViewData["IdPlanta"] = new SelectList(_context.Plantas, "IdPlanta", "NPlanta", ingreso.IdPlanta);
            return View(ingreso);
        }

        // POST: Ingresos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdIngreso,IdPlanta,Producto,Medida,Cantidad,Fecha")] Ingreso ingreso)
        {
            if (id != ingreso.IdIngreso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingreso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngresoExists(ingreso.IdIngreso))
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
            ViewData["IdPlanta"] = new SelectList(_context.Plantas, "IdPlanta", "NPlanta", ingreso.IdPlanta);
            return View(ingreso);
        }

        // GET: Ingresos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos
                .Include(i => i.Planta)
                .FirstOrDefaultAsync(m => m.IdIngreso == id);
            if (ingreso == null)
            {
                return NotFound();
            }

            return View(ingreso);
        }

        // POST: Ingresos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingreso = await _context.Ingresos.FindAsync(id);
            if (ingreso != null)
            {
                _context.Ingresos.Remove(ingreso);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngresoExists(int id)
        {
            return _context.Ingresos.Any(e => e.IdIngreso == id);
        }
    }
}
