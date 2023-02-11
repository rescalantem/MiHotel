using OpenNETCF.MQTT;
using System.Diagnostics;
using System.Text;

namespace MiHotel.Common
{
    public class MensajeRecibido
    {
        public string Topic { get; set; }
        public string Mensaje { get; set; }
    }
    public static class clsMQtt
    {
        private static MQTTClient mqtt;
        public static event EventHandler<MensajeRecibido> MensajeRecibido;
        public static event EventHandler Conectado;
        public static string NombreCliente;
        public static void Conectar(string nombreCliente, string server, int puerto = 1883)
        {
            mqtt = new MQTTClient(server, puerto);
            NombreCliente = nombreCliente;
            Connect();
            mqtt.Connected += Mqtt_Connected;
            mqtt.Disconnected += Mqtt_Disconnected;
            mqtt.MessageReceived += Mqtt_MessageReceived;
        }
        private static void Mqtt_MessageReceived(string topic, QoS qos, byte[] payload)
        {
            Debug.WriteLine("<-" + Encoding.UTF8.GetString(payload));
            MensajeRecibido(null, new MensajeRecibido()
            {
                Mensaje = Encoding.UTF8.GetString(payload),
                Topic = topic
            });
        }
        private static void Mqtt_Disconnected(object sender, EventArgs e)
        {
            Task.Delay(2000);
            Connect();
        }
        private static void Mqtt_Connected(object sender, EventArgs e)
        {
            Debug.WriteLine("clsMQTT -> Conectado a MQTT");
            Conectado(null, null);
        }
        private static void Connect()
        {
            try
            {
                mqtt.Connect(NombreCliente);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"clsMQTT -> Error: {ex.Message}");
            }
        }
        public static void Suscribir(string topic)
        {
            mqtt.Subscriptions.Add(new Subscription(topic));
        }
        public static void Publicar(string topic, string mensaje)
        {
            mqtt.Publish(topic, mensaje, QoS.FireAndForget, false);
            Debug.WriteLine($"clsMQTT -> {topic} - {mensaje}");
        }
    }
}
