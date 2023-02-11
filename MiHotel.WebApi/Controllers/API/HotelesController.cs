using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiHotel.Common.Entities;
using MiHotel.WebApi.Data;

namespace MiHotel.WebApi.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelesController : ControllerBase
    {
        private readonly DataContext _context;

        public HotelesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Hoteles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHoteles()
        {
            if (_context.Hoteles == null)
            {
                return NotFound();
            }
            return await _context.Hoteles.ToListAsync();
        }

        // GET: api/Hoteles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetHotel(int id)
        {
            if (_context.Hoteles == null)
            {
                return NotFound();
            }
            var hotel = await _context.Hoteles.FindAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return hotel;
        }

        // PUT: api/Hoteles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, Hotel hotel)
        {
            if (id != hotel.Id)
            {
                return BadRequest();
            }

            _context.Entry(hotel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Hoteles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(Hotel hotel)
        {
            if (_context.Hoteles == null)
            {
                return Problem("Entity set 'DataContext.Hoteles'  is null.");
            }
            _context.Hoteles.Add(hotel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        // DELETE: api/Hoteles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (_context.Hoteles == null)
            {
                return NotFound();
            }
            var hotel = await _context.Hoteles.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            _context.Hoteles.Remove(hotel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HotelExists(int id)
        {
            return (_context.Hoteles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
