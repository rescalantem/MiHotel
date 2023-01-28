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
            model.Entity<Huesped>().HasIndex(cam => cam.Telefono).IsUnique();
            model.Entity<Estancia>().HasKey(cam => new { cam.HabitacionId, cam.HuespedId });

            //Alta de dato de prueba
            Huesped hues= new Huesped{ Id = 1, Nombre = "Judith Rangel Diaz", Telefono = "1234567890" };
            model.Entity<Huesped>().HasData(hues);

            Hotel hotel = new Hotel
            {
                Id = 1,
                RazonSocial = "Hotel León",
                Habitaciones = new HashSet<Habitacion>
                {
                    new Habitacion
                    {
                        Id = 1,
                        HotelId = 1,
                        Foto = null,
                        NumeroStr = "102",
                        Ocupada = false,
                        Estancias = null
                    },
                    new Habitacion
                    {
                        Id = 2,
                        HotelId = 1,
                        Foto = null,
                        NumeroStr = "202",
                        Ocupada = false,
                        Estancias = null
                    }
                }
            };
            model.Entity<Hotel>().HasData(hotel);

        }
        public DbSet<Hotel> Hoteles => Set<Hotel>();
        public DbSet<Huesped> Huespedes => Set<Huesped>();
        public DbSet<Habitacion> Habitaciones => Set<Habitacion>();
        public DbSet<Estancia> Estancias => Set<Estancia>();
        public DbSet<Acceso> Accesos => Set<Acceso>();
    }
}
