using System.ComponentModel.DataAnnotations;

namespace MiHotel.Common.Entities
{
    public class Habitacion
    {
        public int Id { get; set; }
        [MaxLength(10, ErrorMessage = "El campo {0}, solo acepta {1} caracteres!")]
        public string NumeroStr { get; set; }
        public bool Ocupada { get; set; }
        public byte[] Foto { get; set; }
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public List<Estancia> Estancias { get; set; } = new List<Estancia>();
    }
}
