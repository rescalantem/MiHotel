using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiHotel.Common.Entities;
using MiHotel.WebApi.Data;

namespace MiHotel.WebApi.Controllers
{
    public class AccesosController : Controller
    {
        private readonly DataContext _context;

        public AccesosController(DataContext context)
        {
            _context = context;
        }
        // GET: Accesos
        public async Task<IActionResult> Index(Guid estanciaID)
        {
            var dataContext = _context.Accesos.Include(a => a.Estancia).
                Where(acc => acc.EstanciaId == estanciaID).OrderByDescending(acc => acc.FechaHora);
            return View(await dataContext.ToListAsync());
        }
        // GET: Accesos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accesos == null)
            {
                return NotFound();
            }

            var acceso = await _context.Accesos
                .Include(a => a.Estancia)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (acceso == null)
            {
                return NotFound();
            }

            return View(acceso);
        }
        // GET: Accesos/Create
        public IActionResult Create()
        {
            ViewData["EstanciaId"] = new SelectList(_context.Estancias, "Id", "Id");
            return View();
        }

        // POST: Accesos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EstanciaId,FechaHora")] Acceso acceso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(acceso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstanciaId"] = new SelectList(_context.Estancias, "Id", "Id", acceso.EstanciaId);
            return View(acceso);
        }

        // GET: Accesos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accesos == null)
            {
                return NotFound();
            }

            var acceso = await _context.Accesos.FindAsync(id);
            if (acceso == null)
            {
                return NotFound();
            }
            ViewData["EstanciaId"] = new SelectList(_context.Estancias, "Id", "Id", acceso.EstanciaId);
            return View(acceso);
        }

        // POST: Accesos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EstanciaId,FechaHora")] Acceso acceso)
        {
            if (id != acceso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(acceso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccesoExists(acceso.Id))
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
            ViewData["EstanciaId"] = new SelectList(_context.Estancias, "Id", "Id", acceso.EstanciaId);
            return View(acceso);
        }

        // GET: Accesos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accesos == null)
            {
                return NotFound();
            }

            var acceso = await _context.Accesos
                .Include(a => a.Estancia)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (acceso == null)
            {
                return NotFound();
            }

            return View(acceso);
        }

        // POST: Accesos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accesos == null)
            {
                return Problem("Entity set 'DataContext.Accesos'  is null.");
            }
            var acceso = await _context.Accesos.FindAsync(id);
            if (acceso != null)
            {
                _context.Accesos.Remove(acceso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccesoExists(int id)
        {
            return _context.Accesos.Any(e => e.Id == id);
        }
    }
}
