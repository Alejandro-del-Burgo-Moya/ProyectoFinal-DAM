using ProyectoFinalDAM.BaseDatos;
using ProyectoFinalDAM.Vista;

namespace ProyectoFinalDAM
{
    public partial class AppShell : Shell
    {
        private readonly ShellContent VIniciarSesion = new()
        {
            Title = App.Current!.Resources.TryGetValue("iniciar_sesion", out object iniciar_sesion) ? (string)iniciar_sesion : "error",
            IsVisible = true,

        };
        private readonly ShellContent VIncidencias = new()
        {
            Title = App.Current!.Resources.TryGetValue("incidencias", out object incidencias) ? (string)incidencias : "error",
            IsVisible = true,
        };
        private readonly ShellContent VConfiguracion = new()
        {
            Title = App.Current.Resources.TryGetValue("configuracion", out object configuracion) ? (string)configuracion : "error",
            IsVisible = true,
            Icon = "configuracion.png",
        };
        private readonly FlyoutItem VAdiminstrador = new()
        {
            Title = App.Current.Resources.TryGetValue("administrador", out object administrador) ? (string)administrador : "error",
            IsVisible = true,
        };
        private readonly ShellContent VAyuda = new()
        {
            Title = App.Current.Resources.TryGetValue("ayuda", out object ayuda) ? (string)ayuda : "error",
            IsVisible = true,
        };
        private readonly ShellContent VAcercaDe = new()
        {
            Title = App.Current.Resources.TryGetValue("acerca_de", out object acerca_de) ? (string)acerca_de : "error",
            IsVisible = true,
        };

        private readonly MenuItem MICerrarSesion = new();

        public AppShell()
        {
            InitializeComponent();

            VIniciarSesion.Content = new VistaIniciarSesion(this);
            shell.Items.Add(VIniciarSesion);

            MICerrarSesion.Text = App.Current.Resources.TryGetValue("btn_cerrar_sesion", out object btn_cerrar_sesion) ? (string)btn_cerrar_sesion : "error";
            MICerrarSesion.Clicked += CerrarSesion_Clicked;
        }

        public void SesionIniciada()
        {
            RellenarMenuFlyout();
            AgregarCerrarSesion();
        }

        public void SesionCerrada()
        {
            VaciarMenuFlyout();
        }

        private void RellenarMenuFlyout()
        {
            //Inicialización de las vistas
            VIncidencias.Content = new VistaIncidencias();

            VConfiguracion.Content = new VistaConfiguracion();

            VAdiminstrador.Items.Add(new ShellContent() { Content = new VistaAdministradorUsuarios(), Title = App.Current!.Resources.TryGetValue("ges_usuarios", out object ges_usuarios) ? (string)ges_usuarios : "error" });
            VAdiminstrador.Items.Add(new ShellContent() { Content = new VistaAdministradorIncidencias(), Title = App.Current.Resources.TryGetValue("ges_incidencias", out object ges_incidencias) ? (string)ges_incidencias : "error" });

            VAyuda.Content = new VistaAyuda();

            VAcercaDe.Content = new VistaAcercaDe();

            //Agregar las vistas (decidir que vistas se mostrarán en función del rol del usuario)
            shell.Items.Clear();
            shell.Items.Add(VIncidencias);
            shell.Items.Add(VConfiguracion);
            shell.Items.Add(VAdiminstrador);
            shell.Items.Add(VAyuda);
            shell.Items.Add(VAcercaDe);
        }

        private void VaciarMenuFlyout()
        {
            //Inicialización de las vistas
            VIncidencias.Content = null;

            VConfiguracion.Content = null;

            VAdiminstrador.Items.Add(new ShellContent() { Content = null, Title = App.Current!.Resources.TryGetValue("ges_usuarios", out object ges_usuarios) ? (string)ges_usuarios : "error" });
            VAdiminstrador.Items.Add(new ShellContent() { Content = null, Title = App.Current.Resources.TryGetValue("ges_incidencias", out object ges_incidencias) ? (string)ges_incidencias : "error" });

            VAyuda.Content = null;

            VAcercaDe.Content = null;

            //Agregar las vistas (decidir que vistas se mostrarán en función del rol del usuario)
            shell.Items.Remove(VIncidencias);
            shell.Items.Remove(VConfiguracion);
            shell.Items.Remove(VAdiminstrador);
            shell.Items.Remove(VAyuda);
            shell.Items.Remove(VAcercaDe);

            shell.Items.Clear();
            shell.Items.Add(VIniciarSesion);
        }

        private void CerrarSesion_Clicked(object? sender, EventArgs e)
        {
            VaciarMenuFlyout();
        }

        private void AgregarCerrarSesion()
        {
            shell.Items.Add(MICerrarSesion);
        }
    }
}
