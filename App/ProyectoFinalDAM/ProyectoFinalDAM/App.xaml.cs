using ProyectoFinalDAM.Helpers;

namespace ProyectoFinalDAM
{
    public partial class App : Application
    {
        private static Realms.Sync.App realmApp = Realms.Sync.App.Create(AppConfig.RealmAppId);
        public readonly static int TiempoEspera = 5000;

        public static Realms.Sync.App RealmApp { get => realmApp; }

        public App()
        {
            InitializeComponent();

            realmApp = Realms.Sync.App.Create(AppConfig.RealmAppId);

            MainPage = new AppShell();
        }
    }
}
