using Microsoft.EntityFrameworkCore;
using MiHotel.Common.Entities;
using System.Collections.Generic;

namespace MiHotel.WebApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);
            // Indice y llave compuesta en tabla estancia, relacion muchos a muchos
            model.Entity<Huesped>().HasIndex(hue => hue.Telefono).IsUnique();
            model.Entity<Habitacion>().HasIndex(hab => hab.EspMacAdd).IsUnique();
            model.Entity<Estancia>().HasIndex(est => new { est.HabitacionId, est.HuespedId });

            //Alta de dato de prueba
            Huesped hues= new Huesped{ Id = 1, Nombre = "Judith Rangel Diaz", Telefono = "1234567890" };
            model.Entity<Huesped>().HasData(hues);

            Hotel hot = new Hotel { Id = 1, RazonSocial = "Hotel León"};
            model.Entity<Hotel>().HasData(hot);
            
            Habitacion hab102 = new Habitacion
            {
                Id = 1,
                HotelId = 1,
                EspMacAdd = "00:00:00:00:00:01",
                NumeroStr = "102",
                Ocupada = false,
                Estancias = null
            };
            Habitacion hab202 = new Habitacion
            {
                Id = 2,
                HotelId = 1,
                EspMacAdd = "00:00:00:00:00:02",
                NumeroStr = "202",
                Ocupada = false,
                Estancias = null
            };
            model.Entity<Habitacion>().HasData(hab102,hab202);  
        }
        public DbSet<Hotel> Hoteles => Set<Hotel>();
        public DbSet<Habitacion> Habitaciones => Set<Habitacion>();
        public DbSet<Huesped> Huespedes => Set<Huesped>();
        public DbSet<Estancia> Estancias => Set<Estancia>();
        public DbSet<Acceso> Accesos => Set<Acceso>();
    }
}
