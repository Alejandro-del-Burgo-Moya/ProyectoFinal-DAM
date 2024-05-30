using ProyectoFinalDAM.BaseDatos;
using ProyectoFinalDAM.Vista;

namespace ProyectoFinalDAM
{
    public partial class AppShell : Shell
    {
        public Mongo mongo = new();
        private readonly ShellContent VIncidencias = new() { Title = App.Current!.Resources.TryGetValue("incidencias", out object incidencias) ? (string)incidencias : "error", IsVisible = true, };
        private readonly ShellContent VConfiguracion = new() { Title = App.Current.Resources.TryGetValue("configuracion", out object configuracion) ? (string)configuracion : "error", IsVisible = true, };
        private readonly FlyoutItem VAdiminstrador = new() { Title = App.Current.Resources.TryGetValue("administrador", out object administrador) ? (string)administrador : "error", IsVisible = true, };
        private readonly ShellContent VAyuda = new() { Title = App.Current.Resources.TryGetValue("ayuda", out object ayuda) ? (string)ayuda : "error", IsVisible = true, };
        private readonly ShellContent VAcercaDe = new() { Title = App.Current.Resources.TryGetValue("acerca_de", out object acerca_de) ? (string)acerca_de : "error", IsVisible = true, };

        public AppShell()
        {
            InitializeComponent();


            //Inicialización de las vistas
            VIncidencias.Content = new VistaIncidencias(mongo);

            VConfiguracion.Content = new VistaConfiguracion();

            VAdiminstrador.Items.Add(new ShellContent() { ContentTemplate = new DataTemplate(typeof(VistaAdministradorUsuarios)), Title = App.Current.Resources.TryGetValue("ges_usuarios", out object ges_usuarios) ? (string)ges_usuarios : "error" });
            VAdiminstrador.Items.Add(new ShellContent() { ContentTemplate = new DataTemplate(typeof(VistaAdministradorIncidencias)), Title = App.Current.Resources.TryGetValue("ges_incidencias", out object ges_incidencias) ? (string)ges_incidencias : "error" });

            VAyuda.Content = new VistaAyuda();

            VAcercaDe.Content = new VistaAcercaDe();

            //Agregar las vistas (decidir que vistas se mostrarán en función del rol del usuario)
            shell.Items.Add(VIncidencias);
            shell.Items.Add(VConfiguracion);
            shell.Items.Add(VAdiminstrador);
            shell.Items.Add(VAyuda);
            shell.Items.Add(VAcercaDe);
        }
    }
}
