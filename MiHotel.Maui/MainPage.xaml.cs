using MiHotel.Common;
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
        public MainPage()
        {
            InitializeComponent();
            

            clsMQtt.Conectar(MacEsp , broker);
            clsMQtt.Conectado += Conectado;
            
            barcodeReader.Options = new BarcodeReaderOptions()
            {
                Formats = BarcodeFormat.QrCode
            };

            if (Preferences.ContainsKey("qrLlave"))
            {
                staAbrir.IsVisible = true;
                staScanner.IsVisible = false;
            }
            else
            {
                staAbrir.IsVisible = false;
                staScanner.IsVisible = true;
            }            
        }
        private void CameraBarcodeReaderView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
        {
            Dispatcher.Dispatch(() =>
            {
                if(tmp != e.Results[0].Value.ToString())
                {
                    tmp = e.Results[0].Value.ToString();

                    Preferences.Set("qrLlave", tmp);

                    clsMQtt.Publicar(topic_config, tmp);

                    staScanner.IsVisible = false;
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
            if (mqttConectado) clsMQtt.Publicar(topic_abrir, Preferences.Get("qrLlave","sinllave"));
        }
    }
}