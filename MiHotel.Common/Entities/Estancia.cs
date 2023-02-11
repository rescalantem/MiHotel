using System.ComponentModel.DataAnnotations.Schema;

namespace MiHotel.Common.Entities
{
    public class Estancia
    {
        public Guid Id { get; set; }
        public int HabitacionId { get; set; }
        public Habitacion Habitacion { get; set; }
        public int HuespedId { get; set; }
        public Huesped Huesped { get; set; }
        public DateTime Alta { get; set; }
        public DateTime Baja { get; set; }
        [NotMapped]
        public byte[] Data { get; set; }
    }
}
