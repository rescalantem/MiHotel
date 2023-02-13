using MiHotel.Common;
using MiHotel.Common.Entities;
using MiHotel.Maui.Services;
using System.Runtime.InteropServices;
using ZXing.Net.Maui;

namespace MiHotel.Maui
{
    public partial class MainPage : ContentPage
    {
        bool mqttConectado;
        string broker = "broker.hivemq.com";
        string CelularId = "clienteMAUI";
        string tmp = "";
        private readonly ApiService _apiService;
        private readonly string UrlApi = Application.Current.Resources["UrlAPI"].ToString();        
        public MainPage()
        {
            InitializeComponent();
            _apiService = new ApiService();

            clsMQtt.Conectar(CelularId, broker);
            clsMQtt.Conectado += Conectado;

            barcodeReader.Options = new BarcodeReaderOptions()
            {
                Formats = BarcodeFormat.QrCode
            };

            if (Preferences.ContainsKey("qrLlave"))
            {
                staAbrir.IsVisible = true;
                staScanner.IsVisible = false;
                lblAviso.IsVisible = false;
                CargaDatosEstancia(false);
            }
            else
            {
                staAbrir.IsVisible = false;
                staScanner.IsVisible = true;
                lblAviso.IsVisible = true;
            }            
        }
        async void CargaDatosEstancia(bool inicial)
        {
            var res = await _apiService.GetItemAsync<Estancia>(UrlApi, "/api", $"/Estancias/{Preferences.Get("qrLlave",string.Empty)}", "");
            if (!res.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Alerta!", $"Lo siento no puedo encontrar su llave, verifique en administración", "Ok");
                return;
            }
            Estancia actual = (Estancia)res.Result;

            if (inicial)
            {
                clsMQtt.Publicar($"{actual.Habitacion.EspMacAdd}/config", Preferences.Get("qrLlave",string.Empty));
                Preferences.Set("espMacAdd", actual.Habitacion.EspMacAdd);

                staScanner.IsVisible = false;
                lblAviso.IsVisible = false;
                staAbrir.IsVisible = true;
            }

            lblHotel.Text = actual.Habitacion.Hotel.RazonSocial;
            lblHabitacion.Text = $"Su habitacion es: {actual.Habitacion.NumeroStr}";
            lblVence.Text = $"Vence: {actual.Baja.ToString("g")}";
        }
        private void CameraBarcodeReaderView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
        {
            Dispatcher.Dispatch(() =>
            {
                if (tmp != e.Results[0].Value.ToString())
                {
                    tmp = e.Results[0].Value.ToString();                    
                    
                    Preferences.Set("qrLlave", tmp);
                    Vibration.Vibrate();
                    CargaDatosEstancia(true);

                    //clsMQtt.Publicar(topic_config, tmp);
                    //staScanner.IsVisible = false;
                    //lblAviso.IsVisible = false;
                    //staAbrir.IsVisible = true;
                    
                }
            });
        }
        private async void OnCounterClicked(object sender, EventArgs e)
        {
            await _apiService.DeleteEstanciaAsync(UrlApi, "/api", $"/Estancias/{Preferences.Get("qrLlave",string.Empty)}", "");
            Preferences.Clear();
            staScanner.IsVisible = true;
            lblAviso.IsVisible = true;
            staAbrir.IsVisible = false;
            Vibration.Vibrate();
        }
        private void Conectado(object sender, EventArgs e)
        {
            mqttConectado = true;
            Console.WriteLine("Conectado a: " + CelularId);
        }
        private async void btnAbrir_Clicked(object sender, EventArgs e)
        {
            if (mqttConectado)
            {
                var res = await _apiService.GetItemAsync<Estancia>(UrlApi, "/api", $"/Estancias/{Preferences.Get("qrLlave", string.Empty)}", "");
                if (!res.IsSuccess)
                {
                    await App.Current.MainPage.DisplayAlert("Alerta!", $"Lo siento no puedo encontrar su llave, verifique en administración", "Ok");
                    return;
                }
                Estancia actual = (Estancia)res.Result;
                if (DateTime.Now<=actual.Baja)
                {
                    lblHotel.Text = actual.Habitacion.Hotel.RazonSocial;
                    lblHabitacion.Text = $"Su habitacion es: {actual.Habitacion.NumeroStr}";
                    lblVence.Text = $"Vence: {actual.Baja.ToString("g")}";
                    
                    clsMQtt.Publicar($"{Preferences.Get("espMacAdd", string.Empty)}/abrir", Preferences.Get("qrLlave", "sinllave"));
                    Vibration.Vibrate();
                    Acceso acceso = new Acceso()
                    {
                        EstanciaId = actual.Id, 
                        FechaHora = DateTime.Now,
                    };
                    await _apiService.PostAccesoAsync(UrlApi, "/api", "/Accesos", acceso, "");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alerta!", "Su habitación a vencido, lo siento no puedo abrir la puerta!", "Ok");
                }                
            }
        }
    }
}