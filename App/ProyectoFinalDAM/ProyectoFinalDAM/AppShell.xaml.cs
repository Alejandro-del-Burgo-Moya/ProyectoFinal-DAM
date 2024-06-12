using ProyectoFinalDAM.BaseDatos;
using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Excepciones;
using ProyectoFinalDAM.Vista;

namespace ProyectoFinalDAM
{
    public partial class AppShell : Shell
    {
        public UsuarioLogueado UsuarioLogueado = new();

        private readonly Mongo db_mongo;

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

            db_mongo = new();

            VIniciarSesion.Content = new VistaIniciarSesion(this);
            shell.Items.Add(VIniciarSesion);
            shell.FlyoutBehavior = FlyoutBehavior.Disabled;

            MICerrarSesion.Text = App.Current.Resources.TryGetValue("btn_cerrar_sesion", out object btn_cerrar_sesion) ? (string)btn_cerrar_sesion : "error";
            MICerrarSesion.Clicked += CerrarSesion_Clicked;
        }

        internal void CargarMenuUsuario()
        {
            shell.FlyoutBehavior = FlyoutBehavior.Flyout;
            RellenarMenuFlyout();
            AgregarCerrarSesion();
        }

        internal bool IniciarSesion(string email, string contrasena)
        {
            return db_mongo.IniciarSesion(email, contrasena);
        }

        internal void RegistrarUsuario(Persona persona)
        {
            db_mongo.RegistrarUsuario(persona);
        }

        internal void CerrarSesion()
        {
            //var task = db_mongo.CerrarSesion();
            //task.Wait();

            VaciarMenuFlyout();

            VIniciarSesion.Content = new VistaIniciarSesion(this);
            shell.Items.Add(VIniciarSesion);
            shell.FlyoutBehavior = FlyoutBehavior.Disabled;
        }

        private void RellenarMenuFlyout()
        {
            //Inicialización de las vistas
            VIncidencias.Content = new VistaIncidencias(this);

            VConfiguracion.Content = new VistaConfiguracion();

            VAdiminstrador.Items.Add(new ShellContent() { Content = new VistaAdministradorUsuarios(this), Title = App.Current!.Resources.TryGetValue("ges_usuarios", out object ges_usuarios) ? (string)ges_usuarios : "error" });
            VAdiminstrador.Items.Add(new ShellContent() { Content = new VistaIncidencias(this), Title = App.Current.Resources.TryGetValue("ges_incidencias", out object ges_incidencias) ? (string)ges_incidencias : "error" });

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
            //shell.Items.Remove(VIncidencias);
            //shell.Items.Remove(VConfiguracion);
            //shell.Items.Remove(VAdiminstrador);
            //shell.Items.Remove(VAyuda);
            //shell.Items.Remove(VAcercaDe);

            shell.Items.Clear();
        }

        private void CerrarSesion_Clicked(object? sender, EventArgs e)
        {
            CerrarSesion();
        }

        private void AgregarCerrarSesion()
        {
            shell.Items.Add(MICerrarSesion);
        }

        internal void AgregarPersona(Persona persona)
        {
            db_mongo.CrearPersona(persona);
        }

        internal List<Persona> LeerPersonas()
        {
            return db_mongo.LeerPersonas();
        }

        internal List<Persona> BuscarPersonas(string? nombre = null)
        {
            if (String.IsNullOrWhiteSpace(nombre))
            {
                return db_mongo.LeerPersonas();
            }
            else
            {
                return db_mongo.LeerPersonas().Where(p => p.Nombre.Contains(nombre, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
        }

        internal void CrearIncidencia(Incidencia incidencia)
        {
            db_mongo.CrearIncidencia(incidencia);
        }

        internal void ModificarIncidencia(Incidencia incidencia)
        {
            db_mongo.ModificarIncidencia(incidencia);
        }

        internal List<Incidencia> LeerIncidencias()
        {
            return db_mongo.LeerIncidencias();
        }

        internal List<Incidencia> LeerIncidencias(int? estado = null, int? prioridad = null, int? orden = null, string? nombre = null)
        {
            var lista = db_mongo.LeerIncidencias();

            if (estado != null) { lista = lista.Where(i => i.Estado == estado).ToList(); }

            if (prioridad != null) { lista = lista.Where(i => i.Prioridad == prioridad).ToList(); }

            if (orden != null)
            {
                lista = orden switch
                {
                    0 => [.. lista.OrderBy(i => i.FCreacion)],
                    1 => [.. lista.OrderByDescending(i => i.FCreacion)],
                    2 => [.. lista.OrderByDescending(i => i.Prioridad)],
                    3 => [.. lista.OrderBy(i => i.Prioridad)],
                    4 => [.. lista.OrderByDescending(i => i.Estado)],
                    5 => [.. lista.OrderBy(i => i.Estado)],
                    _ => throw new OrdenIncidenciasException("Se ha introducido un orden no controlado : " + orden),
                };
            }

            if (!string.IsNullOrWhiteSpace(nombre)) { lista = lista.Where(i => i.Nombre!.Contains(nombre, StringComparison.CurrentCultureIgnoreCase)).ToList(); }

            return lista;
        }
    }
}
