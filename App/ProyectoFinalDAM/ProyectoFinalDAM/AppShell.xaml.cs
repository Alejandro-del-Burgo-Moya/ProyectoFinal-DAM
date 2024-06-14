using ProyectoFinalDAM.BaseDatos;
using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Excepciones;
using ProyectoFinalDAM.Vista;

namespace ProyectoFinalDAM
{
    public partial class AppShell : Shell
    {
        public Persona? UsuarioLogueado = null;

        private readonly Mongo db_mongo;

        private readonly ShellContent VIniciarSesion = new()
        {
            Title = Utiles.ExtraerValorDiccionario("iniciar_sesion"),
            IsVisible = true,
            Icon = "iniciar_sesion.png",

        };
        private readonly ShellContent VIncidencias = new()
        {
            Title = Utiles.ExtraerValorDiccionario("incidencias"),
            IsVisible = true,
            Icon = "incidencia.png",
        };
        private readonly ShellContent VMisIncidencias = new()
        {
            Title = Utiles.ExtraerValorDiccionario("incidencias_creadas_por_mi"),
            IsVisible = true,
            Icon = "incidencia.png",
        };
        private readonly ShellContent VMisIncidenciasAsignadas = new()
        {
            Title = Utiles.ExtraerValorDiccionario("mis_incidencias"),
            IsVisible = true,
            Icon = "incidencia.png",
        };
        //private readonly ShellContent VConfiguracion = new()
        //{
        //    Title = Utiles.ExtraerValorDiccionario("configuracion"),
        //    IsVisible = true,
        //    Icon = "configuracion.png",
        //};
        private readonly FlyoutItem VAdiminstrador = new()
        {
            Title = Utiles.ExtraerValorDiccionario("administrador"),
            IsVisible = true,
            Icon = "administrador.png",
        };
        private readonly ShellContent VAyuda = new()
        {
            Title = Utiles.ExtraerValorDiccionario("ayuda"),
            IsVisible = true,
            Icon = "ayuda.png",
        };
        private readonly ShellContent VAcercaDe = new()
        {
            Title = Utiles.ExtraerValorDiccionario("acerca_de"),
            IsVisible = true,
            Icon = "acerca_de.png",
        };

        private readonly MenuItem MICerrarSesion = new();

        public AppShell()
        {
            InitializeComponent();

            db_mongo = new();

            CargarInicioSesion();

            MICerrarSesion.Text = Utiles.ExtraerValorDiccionario("btn_cerrar_sesion");
            MICerrarSesion.IconImageSource = "cerrar_sesion.png";
            MICerrarSesion.Clicked += CerrarSesion_Clicked;
        }

        private void CargarInicioSesion()
        {
            VIniciarSesion.Content = new VistaIniciarSesion(this);
            VAyuda.Content = new VistaAyuda();
            VAcercaDe.Content = new VistaAcercaDe();

            shell.Items.Add(VIniciarSesion);
            shell.Items.Add(VAyuda);
            shell.Items.Add(VAcercaDe);
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

            mensajeUsuarioLogueado.Text = "";
            UsuarioLogueado = null;

            CargarInicioSesion();
        }

        private void RellenarMenuFlyout()
        {
            //Inicialización de las vistas
            VIncidencias.Content = new VistaIncidencias(this);
            VMisIncidencias.Content = new VistaIncidencias(this, incidenciasCreadasPorMi: true);

            if (UsuarioLogueado != null && UsuarioLogueado.Rol != 0) { VMisIncidenciasAsignadas.Content = new VistaIncidencias(this, misIncidenciasAsignadas: true); }

            //VConfiguracion.Content = new VistaConfiguracion();

            if (UsuarioLogueado != null && UsuarioLogueado.Rol == 2)
            {
                VAdiminstrador.Items.Add(new ShellContent() { Content = new VistaAdministradorUsuarios(this), Title = Utiles.ExtraerValorDiccionario("ges_usuarios") });
                VAdiminstrador.Items.Add(new ShellContent() { Content = new VistaIncidencias(this), Title = Utiles.ExtraerValorDiccionario("ges_incidencias") });
            }

            VAyuda.Content = new VistaAyuda();
            VAcercaDe.Content = new VistaAcercaDe();

            //Agregar las vistas (decidir que vistas se mostrarán en función del rol del usuario)
            shell.Items.Clear();
            shell.Items.Add(VIncidencias);
            shell.Items.Add(VMisIncidencias);
            if (UsuarioLogueado != null && UsuarioLogueado.Rol != 0) { shell.Items.Add(VMisIncidenciasAsignadas); }
            //shell.Items.Add(VConfiguracion);
            if (UsuarioLogueado != null && UsuarioLogueado.Rol == 2) { shell.Items.Add(VAdiminstrador); }
            shell.Items.Add(VAyuda);
            shell.Items.Add(VAcercaDe);
        }

        private void VaciarMenuFlyout()
        {
            VIncidencias.Content = null;

            //VConfiguracion.Content = null;

            VAdiminstrador.Items.Clear();

            VAyuda.Content = null;

            VAcercaDe.Content = null;

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
            UsuarioLogueado = db_mongo.IniciarSesion(email, contrasena);
            if (UsuarioLogueado != null) mensajeUsuarioLogueado.Text = Utiles.ExtraerValorDiccionario("mensaje") + UsuarioLogueado.Nombre;
            return UsuarioLogueado != null;
        }

        internal bool RegistrarUsuario(Persona persona)
        {
            return db_mongo.RegistrarUsuario(persona) != -1;
        }

        internal List<Persona> LeerPersonas(string? nombre = null, int? rol = null)
        {
            var lista = db_mongo.LeerPersonas();

            if (!String.IsNullOrWhiteSpace(nombre)) { lista = lista.Where(p => p.Nombre.Contains(nombre, StringComparison.CurrentCultureIgnoreCase)).ToList(); }

            if (rol != null && rol != -1) { lista = lista.Where(p => p.Rol == rol).ToList(); }

            return lista;
        }

        internal List<Incidencia> LeerIncidencias(int? estado = null, int? prioridad = null, int? orden = null, string? nombre = null, bool? misIncidencias = false, bool? incidenciasCreadasPorMi = false)
        {
            var lista = db_mongo.LeerIncidencias();

            if (estado != null) { lista = lista.Where(i => i.Estado == estado).ToList(); }

            if (prioridad != null) { lista = lista.Where(i => i.Prioridad == prioridad).ToList(); }

            if (orden != null)
            {
                lista = orden switch
                {
                    0 => [.. lista.OrderByDescending(i => i.FCreacion)],
                    1 => [.. lista.OrderBy(i => i.FCreacion)],
                    2 => [.. lista.OrderByDescending(i => i.Prioridad)],
                    3 => [.. lista.OrderBy(i => i.Prioridad)],
                    4 => [.. lista.OrderByDescending(i => i.Estado)],
                    5 => [.. lista.OrderBy(i => i.Estado)],
                    _ => throw new OrdenIncidenciasException("Se ha introducido un orden no controlado : " + orden),
                };
            }

            if (!string.IsNullOrWhiteSpace(nombre)) { lista = lista.Where(i => i.Nombre!.Contains(nombre, StringComparison.CurrentCultureIgnoreCase)).ToList(); }

            if (misIncidencias ?? false) { lista = lista.Where(i => UsuarioLogueado!.Equals(i.Asignada)).ToList(); }

            if (incidenciasCreadasPorMi ?? false) { lista = lista.Where(i => UsuarioLogueado!.Equals(i.Creada)).ToList(); }

            return lista;
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

        internal bool ModificarIncidencia(Incidencia incidencia)
        {
            return db_mongo.ModificarIncidencia(incidencia);
        }

        internal void BorrarPersona(Persona persona)
        {
            db_mongo.BorrarUsuario(persona);
        }

        internal void AsignarmeIncidencia(Incidencia incidencia)
        {
            db_mongo.AsignarIncidencia(incidencia, UsuarioLogueado!);
        }

        internal void ResolverIncidencia(Incidencia incidencia)
        {
            db_mongo.ResolverIncidencia(incidencia, UsuarioLogueado!);
        }

    }
}
