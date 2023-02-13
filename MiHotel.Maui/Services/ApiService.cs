using MiHotel.Common.Entities;
using MiHotel.Common.Response;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MiHotel.Maui.Services
{
    public class ApiService : IApiService
    {
        public async Task<Response<object>> GetItemAsync<T>(string urlBase, string servicePrefix, string controller, string token = "")
        {
            try
            {
                HttpClient client = new HttpClient { BaseAddress = new Uri(urlBase) };
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                string prefixControler = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.GetAsync(prefixControler);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<object>
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }
                T obj = JsonConvert.DeserializeObject<T>(result);
                return new Response<object>
                {
                    IsSuccess = true,
                    Result = obj
                };
            }
            catch (Exception ex)
            {
                return new Response<object>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<Response<Acceso>> PostAccesoAsync(string urlAPI, string servicePrefix, string controller, Acceso acceso, string token)
        {
            try
            {
                string stringRequest = JsonConvert.SerializeObject(acceso);
                StringContent content = new StringContent(stringRequest, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient { BaseAddress = new Uri(urlAPI) };
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<Acceso>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                Acceso suje = JsonConvert.DeserializeObject<Acceso>(result);
                return new Response<Acceso>
                {
                    IsSuccess = true,
                    Result = suje
                };
            }
            catch (Exception ex)
            {
                return new Response<Acceso>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<Response<Estancia>> DeleteEstanciaAsync(string urlBase, string servicePrefix, string controller, string token)
        {
            try
            {
                HttpClient client = new HttpClient { BaseAddress = new Uri(urlBase) };
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.DeleteAsync(url);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<Estancia>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                return new Response<Estancia>
                {
                    IsSuccess = true,
                    Result = null
                };
            }
            catch (Exception ex)
            {
                return new Response<Estancia>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
