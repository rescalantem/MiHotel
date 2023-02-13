using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiHotel.Common.Entities;
using MiHotel.WebApi.Data;

namespace MiHotel.WebApi.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstanciasController : ControllerBase
    {
        private readonly DataContext _context;

        public EstanciasController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Estancias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estancia>>> GetEstancias()
        {
            return await _context.Estancias.ToListAsync();
        }

        // GET: api/Estancias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estancia>> GetEstancia(Guid id)
        {
            var estancia = await _context.Estancias.FindAsync(id);
            var huesped = await _context.Huespedes.FindAsync(estancia.HuespedId);
            var habitacion = await _context.Habitaciones.FindAsync(estancia.HabitacionId);
            var hotel = await _context.Hoteles.FindAsync(habitacion.HotelId);

            if (estancia == null) return NotFound();
            estancia.Huesped = huesped;
            estancia.Habitacion = habitacion;
            estancia.Habitacion.Hotel = hotel;
            

            return estancia;
        }

        // PUT: api/Estancias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstancia(Guid id, Estancia estancia)
        {
            if (id != estancia.Id)
            {
                return BadRequest();
            }

            _context.Entry(estancia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstanciaExists(id))
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

        // POST: api/Estancias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Estancia>> PostEstancia(Estancia estancia)
        {
            _context.Estancias.Add(estancia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstancia", new { id = estancia.Id }, estancia);
        }

        // DELETE: api/Estancias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstancia(Guid id)
        {
            var estancia = await _context.Estancias.FindAsync(id);
            if (estancia == null)
            {
                return NotFound();
            }
            var habit = await _context.Habitaciones.FindAsync(estancia.HabitacionId);
            habit.Ocupada = false;

            _context.Estancias.Remove(estancia);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool EstanciaExists(Guid id)
        {
            return _context.Estancias.Any(e => e.Id == id);
        }
    }
}
