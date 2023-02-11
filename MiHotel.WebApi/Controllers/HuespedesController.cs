using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiHotel.Common.Entities;
using MiHotel.WebApi.Data;

namespace MiHotel.WebApi.Controllers
{
    public class HuespedesController : Controller
    {
        private readonly DataContext _context;

        public HuespedesController(DataContext context)
        {
            _context = context;
        }

        // GET: Huespedes
        public async Task<IActionResult> Index()
        {
            return _context.Huespedes != null ?
                        View(await _context.Huespedes.ToListAsync()) :
                        Problem("Entity set 'DataContext.Huespedes'  is null.");
        }

        // GET: Huespedes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Huespedes == null)
            {
                return NotFound();
            }

            var huesped = await _context.Huespedes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (huesped == null)
            {
                return NotFound();
            }

            return View(huesped);
        }

        // GET: Huespedes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Huespedes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Telefono")] Huesped huesped)
        {
            if (ModelState.IsValid)
            {
                _context.Add(huesped);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(huesped);
        }

        // GET: Huespedes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Huespedes == null)
            {
                return NotFound();
            }

            var huesped = await _context.Huespedes.FindAsync(id);
            if (huesped == null)
            {
                return NotFound();
            }
            return View(huesped);
        }

        // POST: Huespedes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Telefono")] Huesped huesped)
        {
            if (id != huesped.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(huesped);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HuespedExists(huesped.Id))
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
            return View(huesped);
        }

        // GET: Huespedes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Huespedes == null)
            {
                return NotFound();
            }

            var huesped = await _context.Huespedes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (huesped == null)
            {
                return NotFound();
            }

            return View(huesped);
        }

        // POST: Huespedes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Huespedes == null)
            {
                return Problem("Entity set 'DataContext.Huespedes'  is null.");
            }
            var huesped = await _context.Huespedes.FindAsync(id);
            if (huesped != null)
            {
                _context.Huespedes.Remove(huesped);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HuespedExists(int id)
        {
            return (_context.Huespedes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
