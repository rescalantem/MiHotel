using System.ComponentModel.DataAnnotations;

namespace MiHotel.Common.Entities
{
    public class Huesped
    {
        public int Id { get; set; }
        [MaxLength(200, ErrorMessage = "El campo {0}, solo acepta {1} caracteres!")]
        public string Nombre { get; set; }
        [MaxLength(10, ErrorMessage = "El campo {0}, solo acepta {1} caracteres!")]
        public string Telefono { get; set; }
        public List<Estancia> Estancias { get; set; } = new List<Estancia>();
    }
}
