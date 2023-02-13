using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiHotel.Common.Entities;
using MiHotel.WebApi.Data;

namespace MiHotel.WebApi.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccesosController : ControllerBase
    {
        private readonly DataContext _context;

        public AccesosController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Accesos
        [HttpGet("{estanciaId}")]
        public async Task<ActionResult<IEnumerable<Acceso>>> GetAccesos(Guid estanciaId)
        {
            return await _context.Accesos.Where(acc => acc.EstanciaId == estanciaId).
                OrderByDescending(acc => acc.FechaHora).ToListAsync();
        }

        // GET: api/Accesos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Acceso>> GetAcceso(int id)
        {
            var acceso = await _context.Accesos.FindAsync(id);

            if (acceso == null)
            {
                return NotFound();
            }

            return acceso;
        }

        // PUT: api/Accesos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAcceso(int id, Acceso acceso)
        {
            if (id != acceso.Id)
            {
                return BadRequest();
            }

            _context.Entry(acceso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccesoExists(id))
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

        // POST: api/Accesos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Acceso>> PostAcceso(Acceso acceso)
        {
            _context.Accesos.Add(acceso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAcceso", new { id = acceso.Id }, acceso);
        }

        // DELETE: api/Accesos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAcceso(int id)
        {
            var acceso = await _context.Accesos.FindAsync(id);
            if (acceso == null)
            {
                return NotFound();
            }

            _context.Accesos.Remove(acceso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccesoExists(int id)
        {
            return _context.Accesos.Any(e => e.Id == id);
        }
    }
}
