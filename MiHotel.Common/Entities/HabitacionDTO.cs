using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiHotel.Common.Entities
{
    public class HabitacionDTO
    {
        public int HotelId { get; set; }
        public string NumeroStr { get; set; }
        public bool Ocupada { get; set; }
    }
}
