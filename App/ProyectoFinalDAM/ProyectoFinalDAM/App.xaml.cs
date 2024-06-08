using ProyectoFinalDAM.Helpers;

namespace ProyectoFinalDAM
{
    public partial class App : Application
    {
        public readonly static Realms.Sync.App RealmApp = Realms.Sync.App.Create(AppConfig.RealmAppId);
        public readonly static int TiempoEspera = 5000;
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
