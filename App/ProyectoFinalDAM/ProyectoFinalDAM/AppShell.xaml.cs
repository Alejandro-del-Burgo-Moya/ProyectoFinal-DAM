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
            Title = Utiles.ExtraerValorDiccionario("iniciar_sesion"),
            IsVisible = true,

        };
        private readonly ShellContent VIncidencias = new()
        {
            Title = Utiles.ExtraerValorDiccionario("incidencias"),
            IsVisible = true,
        };
        private readonly ShellContent VConfiguracion = new()
        {
            Title = Utiles.ExtraerValorDiccionario("configuracion"),
            IsVisible = true,
            Icon = "configuracion.png",
        };
        private readonly FlyoutItem VAdiminstrador = new()
        {
            Title = Utiles.ExtraerValorDiccionario("administrador"),
            IsVisible = true,
        };
        private readonly ShellContent VAyuda = new()
        {
            Title = Utiles.ExtraerValorDiccionario("ayuda"),
            IsVisible = true,
        };
        private readonly ShellContent VAcercaDe = new()
        {
            Title = Utiles.ExtraerValorDiccionario("acerca_de"),
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

            MICerrarSesion.Text = Utiles.ExtraerValorDiccionario("btn_cerrar_sesion");
            MICerrarSesion.Clicked += CerrarSesion_Clicked;
        }

        internal void CargarMenuUsuario()
        {
            shell.FlyoutBehavior = FlyoutBehavior.Flyout;
            RellenarMenuFlyout();
            AgregarCerrarSesion();
        }

        internal void CerrarSesion()
        {
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

            VAdiminstrador.Items.Add(new ShellContent() { Content = new VistaAdministradorUsuarios(this), Title = Utiles.ExtraerValorDiccionario("ges_usuarios") });
            VAdiminstrador.Items.Add(new ShellContent() { Content = new VistaIncidencias(this), Title = Utiles.ExtraerValorDiccionario("ges_incidencias") });

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

            VAdiminstrador.Items.Add(new ShellContent() { Content = null, Title = Utiles.ExtraerValorDiccionario("ges_usuarios") });
            VAdiminstrador.Items.Add(new ShellContent() { Content = null, Title = Utiles.ExtraerValorDiccionario("ges_incidencias") });

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


        internal bool IniciarSesion(string email, string contrasena)
        {
            return db_mongo.IniciarSesion(email, contrasena);
        }

        internal void RegistrarUsuario(Persona persona)
        {
            db_mongo.RegistrarUsuario(persona);
        }

        internal List<Incidencia> LeerIncidencias()
        {
            return db_mongo.LeerIncidencias();
        }

        internal List<Persona> LeerPersonas()
        {
            return db_mongo.LeerPersonas();
        }

        internal void AgregarPersona(Persona persona)
        {
            db_mongo.CrearPersona(persona);
        }

        internal void CrearIncidencia(Incidencia incidencia)
        {
            db_mongo.CrearIncidencia(incidencia);
        }

        internal void ModificarPersona(Persona persona)
        {
            db_mongo.ModificarPersona(persona);
        }

        internal void ModificarIncidencia(Incidencia incidencia)
        {
            db_mongo.ModificarIncidencia(incidencia);
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
