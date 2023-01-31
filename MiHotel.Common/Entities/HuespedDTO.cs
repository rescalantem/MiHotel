using System.ComponentModel.DataAnnotations;

namespace MiHotel.Common.Entities
{
    public class HuespedDTO
    {
        [MaxLength(150,ErrorMessage = "El campo {0}, solo acepta {1} caracteres")]
        public string Nombre { get; set; }
        [MaxLength(10, ErrorMessage = "El campo {0}, solo acepta {1} caracteres")]
        public string Telefono { get; set; }
    }
}
