using Gma.QrCodeNet.Encoding.Windows.Render;
using Gma.QrCodeNet.Encoding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiHotel.Common.Entities;
using MiHotel.WebApi.Data;
using System.Drawing.Imaging;
using System.Drawing;
using MiHotel.Common;

namespace MiHotel.WebApi.Controllers
{
    public class EstanciasController : Controller
    {
        private readonly DataContext _context;
        private bool mqttConectado;
        private string broker = "broker.hivemq.com";

        public EstanciasController(DataContext context)
        {
            _context = context;            
        }

        // GET: Estancias
        public async Task<IActionResult> Index(int HotelId)
        {
            var dataContext = _context.Estancias.Include(e => e.Habitacion).Include(e => e.Huesped)
                .Where(hot => hot.Habitacion.HotelId == HotelId).ToList();

            foreach(var sta in dataContext)
            {
                var qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                var qrCode = qrEncoder.Encode(sta.Id.ToString());

                var renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);
                using (var stream = new MemoryStream())
                {
                    renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
                    sta.Data = stream.ToArray();
                }
            }            
            return View(dataContext);
        }

        // GET: Estancias/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Estancias == null)
            {
                return NotFound();
            }

            var estancia = await _context.Estancias
                .Include(e => e.Habitacion)
                .Include(e => e.Huesped)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estancia == null)
            {
                return NotFound();
            }

            return View(estancia);
        }

        // GET: Estancias/Create
        public IActionResult Create(int HotelId)
        {
            var habits = _context.Habitaciones.Where(h => !h.Ocupada && h.HotelId == HotelId);
            ViewData["Habitaciones"] = new SelectList(habits, "Id", "NumeroStr");
            var huesps = _context.Huespedes.OrderBy(h => h.Nombre);
            huesps.Append(new Huesped { Nombre = "[Seleccione]", Telefono = "" });
            ViewData["Huespedes"] = new SelectList(huesps, "Id", "Nombre");
            
            return View();
        }
        private void Conectado(object sender, EventArgs e)
        {
            mqttConectado = true;
            Console.WriteLine("Conectado al broker!");
        }
        // POST: Estancias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HabitacionId,HuespedId,Alta,Baja")] Estancia estancia)
        {
            if (ModelState.IsValid)
            {
                var habits = await _context.Habitaciones.FindAsync(estancia.HabitacionId);
                habits.Ocupada = true;
                estancia.Id = Guid.NewGuid();
                _context.Add(estancia);
                await _context.SaveChangesAsync();

                clsMQtt.Conectar(estancia.Id.ToString(), broker);
                

                if (mqttConectado) clsMQtt.Publicar($"{habits.EspMacAdd}/config", estancia.Id.ToString());

                return RedirectToAction(nameof(Index));
            }

            ViewData["Habitaciones"] = new SelectList(_context.Habitaciones, "Id", "Id", estancia.HabitacionId);
            ViewData["Huespedes"] = new SelectList(_context.Huespedes, "Id", "Id", estancia.HuespedId);
            return View(estancia);
        }

        // GET: Estancias/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Estancias == null)
            {
                return NotFound();
            }

            var estancia = await _context.Estancias.FindAsync(id);
            if (estancia == null)
            {
                return NotFound();
            }
            ViewData["HabitacionId"] = new SelectList(_context.Habitaciones, "Id", "Id", estancia.HabitacionId);
            ViewData["HuespedId"] = new SelectList(_context.Huespedes, "Id", "Id", estancia.HuespedId);
            return View(estancia);
        }

        // POST: Estancias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,HabitacionId,HuespedId,Alta,Baja")] Estancia estancia)
        {
            if (id != estancia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estancia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstanciaExists(estancia.Id))
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
            ViewData["HabitacionId"] = new SelectList(_context.Habitaciones, "Id", "Id", estancia.HabitacionId);
            ViewData["HuespedId"] = new SelectList(_context.Huespedes, "Id", "Id", estancia.HuespedId);
            return View(estancia);
        }

        // GET: Estancias/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Estancias == null) return NotFound();
            
            var estancia = await _context.Estancias
                .Include(e => e.Habitacion)
                .Include(e => e.Huesped)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estancia == null) return NotFound();            

            return View(estancia);
        }

        // POST: Estancias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Estancias == null)return Problem("Entity set 'DataContext.Estancias'  is null.");
            
            var estancia = await _context.Estancias.FindAsync(id);
            if (estancia != null) _context.Estancias.Remove(estancia);
            var habDispo = await _context.Habitaciones.FindAsync(estancia.HabitacionId);
            habDispo.Ocupada = false;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstanciaExists(Guid id)
        {
            return (_context.Estancias?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
