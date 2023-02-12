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
        string MacEsp = "clienteMAUI";
        string topic_config = "E0:5A:1B:75:A7:44/config";
        string topic_abrir = "E0:5A:1B:75:A7:44/abrir";
        string tmp = "";
        private readonly ApiService _apiService;
        private readonly string UrlApi = Application.Current.Resources["UrlAPI"].ToString();

        //private readonly string token = JsonConvert.DeserializeObject<TokenResponse>(Preferences.Get("Token", string.Empty)).Token;
        public MainPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            
            //Preferences.Set("qrLlave", "f7483b94-5580-4927-911b-bc9ff6eeec33");

            clsMQtt.Conectar(MacEsp, broker);
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
                CargaDatos();
            }
            else
            {
                staAbrir.IsVisible = false;
                staScanner.IsVisible = true;
                lblAviso.IsVisible = true;
            }            
        }
        async void CargaDatos()
        {
            var res = await _apiService.GetItemAsync<Estancia>(UrlApi, "/api", $"/Estancias/{Preferences.Get("qrLlave",string.Empty)}", "");
            if (!res.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Alerta", $"Error: {res.Message}", "Ok");
                return;
            }
            Estancia tmp = (Estancia)res.Result;

            lblHotel.Text = "Mi Hotel";
            lblHabitacion.Text = $"Su habitacion es: {tmp.Habitacion.NumeroStr}";
            lblVence.Text = $"Vence: {tmp.Baja.ToString("g")}";

        }
        private void CameraBarcodeReaderView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
        {
            Dispatcher.Dispatch(() =>
            {
                if (tmp != e.Results[0].Value.ToString())
                {
                    tmp = e.Results[0].Value.ToString();

                    Preferences.Set("qrLlave", tmp);

                    clsMQtt.Publicar(topic_config, tmp);

                    staScanner.IsVisible = false;
                    lblAviso.IsVisible = false;
                    staAbrir.IsVisible = true;

                    Vibration.Vibrate();

                    Console.WriteLine($"#### Enviando: {tmp}");
                }

            });
        }
        private void OnCounterClicked(object sender, EventArgs e)
        {
            Preferences.Clear();
            tmp = "";
            staScanner.IsVisible = true;
            lblAviso.IsVisible = true;
            staAbrir.IsVisible = false;
            Vibration.Vibrate();
        }
        private void Conectado(object sender, EventArgs e)
        {
            mqttConectado = true;
            Console.WriteLine("Conectado a: " + MacEsp);
        }
        private void btnAbrir_Clicked(object sender, EventArgs e)
        {
            if (mqttConectado) clsMQtt.Publicar(topic_abrir, Preferences.Get("qrLlave", "sinllave"));
        }
    }
}