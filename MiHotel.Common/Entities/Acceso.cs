namespace MiHotel.Common.Entities
{
    public class Acceso
    {
        public int Id { get; set; }
        public Guid EstanciaId { get; set; }
        public Estancia Estancia { get; set; }
        public DateTime FechaHora { get; set; }
    }
}
