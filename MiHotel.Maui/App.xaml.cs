using MiHotel.Maui.Services;

namespace MiHotel.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
             MainPage = new AppShell();
        }
    }
}