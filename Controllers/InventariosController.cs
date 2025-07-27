using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControlBodega.Data;
using ControlBodega.Models;

namespace ControlBodega.Controllers
{
    public class InventariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Inventarios
        public async Task<IActionResult> Index()
        {
            var inventarios = _context.Inventarios.Include(i => i.Planta);
            return View(await inventarios.ToListAsync());
        }

        // GET: Inventarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventarios
                .Include(i => i.Planta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventario == null)
            {
                return NotFound();
            }

            return View(inventario);
        }

        // GET: Inventarios/Create
        public IActionResult Create()
        {
            ViewData["IdPlanta"] = new SelectList(_context.Plantas, "IdPlanta", "NPlanta");
            return View();
        }

        // POST: Inventarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Producto,Medida,Precio,IdPlanta")] Inventario inventario)
        {
            Console.WriteLine($"[LOG] POST Create Inventario ejecutado. Producto: {inventario?.Producto}");
            if (ModelState.IsValid)
            {
                Console.WriteLine("[LOG] ModelState válido. Se intenta agregar el inventario...");
                _context.Add(inventario);
                await _context.SaveChangesAsync();
                Console.WriteLine($"[LOG] Inventario creado con IdPlanta: {inventario.IdPlanta}");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Console.WriteLine("[LOG] ModelState inválido. No se crea el inventario.");
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"[ERROR] Campo: {key}, Error: {error.ErrorMessage}");
                    }
                }
            }
            ViewData["IdPlanta"] = new SelectList(_context.Plantas, "IdPlanta", "NPlanta", inventario.IdPlanta);
            return View(inventario);
        }
        

        // GET: Inventarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario == null)
            {
                return NotFound();
            }
            ViewData["IdPlanta"] = new SelectList(_context.Plantas, "IdPlanta", "NPlanta", inventario.IdPlanta);
            return View(inventario);
        }

        // POST: Inventarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Producto,Medida,Precio,IdPlanta")] Inventario inventario)
        {
            if (id != inventario.Id)
            {
                return NotFound();
            }

            Console.WriteLine($"[LOG] POST Edit Inventario ejecutado. Producto: {inventario?.Producto}, Precio: {inventario?.Precio}");
            if (ModelState.IsValid)
            {
                Console.WriteLine($"[LOG] ModelState válido. Precio recibido: {inventario.Precio}");
                try
                {
                    _context.Update(inventario);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"[LOG] Inventario actualizado. Precio guardado: {inventario.Precio}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventarioExists(inventario.Id))
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
            else
            {
                Console.WriteLine("[LOG] ModelState inválido. No se edita el inventario.");
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"[ERROR] Campo: {key}, Error: {error.ErrorMessage}");
                    }
                }
            }
            ViewData["IdPlanta"] = new SelectList(_context.Plantas, "IdPlanta", "NPlanta", inventario.IdPlanta);
            return View(inventario);
        }

        // GET: Inventarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventarios
                .Include(i => i.Planta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventario == null)
            {
                return NotFound();
            }

            return View(inventario);
        }

        // POST: Inventarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario != null)
            {
                _context.Inventarios.Remove(inventario);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventarioExists(int id)
        {
            return _context.Inventarios.Any(e => e.Id == id);
        }
    }
}
