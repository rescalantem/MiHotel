using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiHotel.Common.Entities;
using MiHotel.WebApi.Data;

namespace MiHotel.WebApi.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class HuespedesController : ControllerBase
    {
        private DataContext context;
        private readonly IMapper mapper;

        public HuespedesController(DataContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }
        [HttpGet("ByHotelId/{hotelId}")]
        public async Task<ActionResult<IEnumerable<Huesped>>> ByHotelId(int hotelId)
        {
            var hues = await context.Huespedes.
                       Join(context.Estancias, hue => hue.Id, est => est.HuespedId, (hue, est) => new {hue, est}).
                       Where(res => res.est.Habitacion.HotelId == hotelId).
                       Select(res => res.hue).
                       Distinct().
                       ToListAsync();

            if (hues == null)
                return NotFound();
            else
                return Ok(hues);
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<Huesped>> Get(int Id)
        {
            var hue = await context.Huespedes.FindAsync(Id); 
            if(hue == null)            
                return NotFound();
            else
                return Ok(hue);
        }
        [HttpGet("ByTelefono/{telefono}")]
        public async Task<ActionResult<Huesped>> ByTelefono(string telefono)
        {
            var hue = await context.Huespedes.FirstOrDefaultAsync(hue => hue.Equals(telefono));
            if (hue == null)
                return NotFound();
            else
                return Ok(hue);
        }
        [HttpPost]
        public async Task<ActionResult<Huesped>> Post(HuespedDTO nuevoDTO)
        {
            if(await context.Huespedes.AnyAsync(h => h.Telefono == nuevoDTO.Telefono)) return Problem("Teléfono duplicado");

            var nuevo = mapper.Map<Huesped>(nuevoDTO);

            context.Huespedes.Add(nuevo);

            await context.SaveChangesAsync();

            return Ok(nuevo);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Huesped>> Put(int id, HuespedDTO modi)
        {
            var hue = await context.Huespedes.FindAsync(id);
            if (hue == null) return NotFound();
            hue.Telefono = modi.Telefono;
            hue.Nombre = modi.Nombre;            
            await context.SaveChangesAsync(true);
            return Ok(modi);
        }
    }
}
