using AutoMapper;
using MiHotel.Common.Entities;

namespace MiHotel.WebApi.Helpers
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<HuespedDTO,Huesped>();
            CreateMap<HabitacionDTO,Habitacion>();
        }
    }
}
