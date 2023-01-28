using System.ComponentModel.DataAnnotations;

namespace MiHotel.Common.Entities
{
    public class Hotel
    {
        public int Id { get; set; }
        [MaxLength(150, ErrorMessage = "El campo {0}, solo acepta {1} caracteres")]
        public string RazonSocial { get; set; } = null!;
        public HashSet<Habitacion> Habitaciones { get; set; } = new HashSet<Habitacion>();
    }
}
