using MiHotel.Common.Entities;
using MiHotel.Common.Response;

namespace MiHotel.Maui.Services
{
    public interface IApiService
    {
        Task<Response<Estancia>> DeleteEstanciaAsync(string urlBase, string servicePrefix, string controller, string token);
        Task<Response<object>> GetItemAsync<T>(string urlBase, string servicePrefix, string controller, string token);
    }
}