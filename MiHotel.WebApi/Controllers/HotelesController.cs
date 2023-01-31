using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiHotel.Common.Entities;
using MiHotel.WebApi.Data;

namespace MiHotel.WebApi.Controllers
{
    public class HotelesController : Controller
    {
        private readonly DataContext _context;

        public HotelesController(DataContext context)
        {
            _context = context;
        }

        // GET: Hoteles
        public async Task<IActionResult> Index()
        {
            return _context.Hoteles != null ?
                        View(await _context.Hoteles.Include("Habitaciones").ToListAsync()) :
                        Problem("Entity set 'DataContext.Hoteles'  is null.");
        }

        // GET: Hoteles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hoteles == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hoteles.Include("Habitaciones")
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // GET: Hoteles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hoteles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RazonSocial")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // GET: Hoteles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Hoteles == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hoteles.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        // POST: Hoteles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RazonSocial")] Hotel hotel)
        {
            if (id != hotel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.Id))
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
            return View(hotel);
        }

        // GET: Hoteles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hoteles == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hoteles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // POST: Hoteles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hoteles == null)
            {
                return Problem("Entity set 'DataContext.Hoteles'  is null.");
            }
            var hotel = await _context.Hoteles.FindAsync(id);
            if (hotel != null)
            {
                _context.Hoteles.Remove(hotel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelExists(int id)
        {
            return (_context.Hoteles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
