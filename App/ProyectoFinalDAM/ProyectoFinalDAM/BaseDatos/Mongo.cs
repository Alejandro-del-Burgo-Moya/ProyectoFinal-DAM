using MongoDB.Bson;
using MongoDB.Driver;
using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Enums;
using ProyectoFinalDAM.Modelo.Excepciones;
using Realms;
using Realms.Sync;
using MongoClient = MongoDB.Driver.MongoClient;

namespace ProyectoFinalDAM.BaseDatos
{
    public static class Mongo
    {
        //mongodb+srv://adburgom01:b8nl7320c@proyectofinaldam.q49ehh3.mongodb.net/?retryWrites=true&w=majority&appName=ProyectoFinalDAM
        //mongodb+srv://adburgom01:b8nl7320c@proyectofinaldam.q49ehh3.mongodb.net/test?retryWrites=true&w=majority
        private static readonly string URIConexion = "mongodb+srv://adburgom01:b8nl7320c@proyectofinaldam.q49ehh3.mongodb.net/?retryWrites=true&w=majority&appName=ProyectoFinalDAM";
        //private static readonly string URIConexion = "mongodb://adburgom01:b8nl7320c@proyectofinaldam.q49ehh3.mongodb.net/?retryWrites=true&w=majority&appName=ProyectoFinalDAM";
        private static readonly string nombreBaseDatos = "ProyectoFinalDAM";
        private static readonly string coleccionIncidencias = "Incidencias";
        private static readonly string coleccionPersonas = "Personas";
        private static readonly string idAppRealm = "app_proyectofinaldam-orwbcjn";
        //private static MongoClient? _client = null;
        //private static IMongoCollection<Incidencia>? _coleccionIncidencias;
        //private static IMongoCollection<Persona>? _coleccionPersonas;

        private static Realms.Sync.App? app;
        private static User? user;
        private static PartitionSyncConfiguration? config;
        private static Realm? realm;

        private static bool appInicializada = false;
        private static bool realmInicializado = false;

        private static List<Incidencia> listaIncidencias = [];
        private static List<Persona> listaPersonas = [];

        //public Mongo()
        //{
        //    var settings = MongoClientSettings.FromConnectionString(URIConexion);
        //    settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        //    _client = new MongoClient(settings);
        //    _coleccionIncidencias = _client.GetDatabase(nombreBaseDatos).GetCollection<Incidencia>(coleccionIncidencias);
        //    _coleccionPersonas = _client.GetDatabase(nombreBaseDatos).GetCollection<Persona>(coleccionPersonas);
        //}

        private static void InicializarMongo()
        {
            app = Realms.Sync.App.Create(idAppRealm);
            var appConfig = new AppConfiguration(idAppRealm)
            {
                DefaultRequestTimeout = TimeSpan.FromMilliseconds(1500),
                BaseUri = new Uri(URIConexion)
            };
            app = Realms.Sync.App.Create(appConfig);

            //var settings = MongoClientSettings.FromConnectionString(URIConexion);
            //settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            //_client = new MongoClient(settings);
            //_coleccionIncidencias = _client.GetDatabase(nombreBaseDatos).GetCollection<Incidencia>(coleccionIncidencias);
            //_coleccionPersonas = _client.GetDatabase(nombreBaseDatos).GetCollection<Persona>(coleccionPersonas);

            appInicializada = true;
        }

        public static async Task IniciarSesion(string email, string contrasena)
        {
            if (!appInicializada) { InicializarMongo(); }
            //if (app!.CurrentUser == null)
            //{
            //    // App must be online for user to authenticate
            //    user = await app!.LogInAsync(Credentials.EmailPassword(email, contrasena));
            //    config = new PartitionSyncConfiguration("_part", user);
            //    realm = await Realm.GetInstanceAsync(config);
            //}
            //else
            //{
            //    // This works whether online or offline
            //    user = app.CurrentUser;
            //    config = new PartitionSyncConfiguration("_part", user);
            //    realm = Realm.GetInstance(config);
            //}
            user = await app!.LogInAsync(Credentials.ApiKey("7f39b673-e5a2-4da8-9704-a14a85883a1d"));
            config = new PartitionSyncConfiguration("_part", user);
            realm = await Realm.GetInstanceAsync(config);
            realmInicializado = true;
        }

        public static async void IniciarSesion(Persona persona)
        {
            await IniciarSesion(persona.Email!, persona.Contrasena!);
        }

        public static void CerrarSesion()
        {

        }



        #region "Incidencias"

        public static List<Incidencia> LeerIncidencias()
        {
            if (!realmInicializado) { throw new ErrorInicioSesionException(); }
            listaIncidencias = [.. realm!.All<Incidencia>()];
            return listaIncidencias;
            //return [.. _coleccionIncidencias.AsQueryable().Where(i => i.Nombre != "")];
        }

        public static List<Incidencia> LeerIncidenciasFiltroOrdenNombre(int? estado = null, int? prioridad = null, int? orden = null, string? nombre = null)
        {
            if (!realmInicializado) { throw new ErrorInicioSesionException(); }

            listaIncidencias = [.. realm!.All<Incidencia>()];

            if (estado != null) { listaIncidencias = listaIncidencias.Where(i => i.Estado == estado).ToList(); }

            if (prioridad != null) { listaIncidencias = listaIncidencias.Where(i => i.Prioridad == prioridad).ToList(); }

            if (orden != null)
            {
                listaIncidencias = orden switch
                {
                    0 => [.. listaIncidencias.OrderBy(i => i.FCreacion)],
                    1 => [.. listaIncidencias.OrderByDescending(i => i.FCreacion)],
                    2 => [.. listaIncidencias.OrderByDescending(i => i.Prioridad)],
                    3 => [.. listaIncidencias.OrderBy(i => i.Prioridad)],
                    4 => [.. listaIncidencias.OrderByDescending(i => i.Estado)],
                    5 => [.. listaIncidencias.OrderBy(i => i.Estado)],
                    _ => throw new OrdenIncidenciasException("Se ha introducido un orden no controlado : " + orden),
                };
            }

            if (nombre != null) { listaIncidencias = listaIncidencias.Where(i => i.Nombre!.Contains(nombre, StringComparison.CurrentCultureIgnoreCase)).ToList(); }

            return listaIncidencias;
        }

        //public static List<Incidencia> BuscarIncidencia(string nombre)
        //{
        //    if (!realmInicializado) { throw new ErrorInicioSesionException(); }
        //    listaIncidencias = listaIncidencias.Where(i => i.Nombre!.Contains(nombre, StringComparison.CurrentCultureIgnoreCase)).ToList();
        //    return listaIncidencias;
        //    //return [.. _coleccionIncidencias.AsQueryable().Where(i => i.Nombre!.Contains(nombre, StringComparison.CurrentCultureIgnoreCase))];
        //}

        public static async Task CrearIncidenciaAsync(Incidencia incidencia)
        {
            if (!realmInicializado) { throw new ErrorInicioSesionException(); }
            await realm!.WriteAsync(() => { realm.Add(incidencia); });
            //await _coleccionIncidencias!.InsertOneAsync(incidencia);
        }

        public static async Task BorrarIncidencia(ObjectId id)
        {
            //await _coleccionIncidencias.DeleteOneAsync(i => i.Id == id);
            if (!realmInicializado) { throw new ErrorInicioSesionException(); }

            var temp = LeerIncidencias();
            Incidencia incidenciaBorrar = (Incidencia)temp.Select(i => i.Id == id);

            await realm!.WriteAsync(() => { realm.Remove(incidenciaBorrar); });

        }

        public static async Task BorrarIncidencia(Incidencia incidencia)
        {
            //await BorrarIncidencia(incidencia.Id);
            if (!realmInicializado) { throw new ErrorInicioSesionException(); }
            await realm!.WriteAsync(() => { realm.Remove(incidencia); });
        }

        public static async Task AsignarIncidencia(Incidencia incidencia, Persona persona)
        {
            if (!realmInicializado) { throw new ErrorInicioSesionException(); }

            //var filtro = Builders<Incidencia>.Filter.Eq(i => i.Id, incidencia.Id);
            //var actualizacion = Builders<Incidencia>.Update
            //    .Set(i => i.Asignada, persona)
            //    .Set(i => i.FAsignacion, DateTime.Now)
            //    .Set(i => i.Estado, ((int)Estado.Asignada));

            await realm!.WriteAsync(() =>
            {
                incidencia.Asignada = persona;
                incidencia.FAsignacion = DateTimeOffset.Now;
                incidencia.Estado = (int)Estado.Asignada;
            });

            //await _coleccionIncidencias!.UpdateOneAsync(filtro, actualizacion);
        }

        public static async Task ResolverIncidenciaAdmin(Incidencia incidencia)
        {
            if (!realmInicializado) { throw new ErrorInicioSesionException(); }

            //var filtro = Builders<Incidencia>.Filter.Eq(i => i.Id, incidencia.Id);
            //var actualizacion = Builders<Incidencia>.Update
            //    .Set(i => i.Resuelta, incidencia.Asignada)
            //    .Set(i => i.FResolucion, DateTime.Now)
            //    .Set(i => i.Estado, (int)Estado.Resuelta);

            await realm!.WriteAsync(() =>
            {
                incidencia.Resuelta = incidencia.Asignada;
                incidencia.FResolucion = DateTimeOffset.Now;
                incidencia.Estado = (int)Estado.Resuelta;
            });

            //await _coleccionIncidencias!.UpdateOneAsync(filtro, actualizacion);
        }

        #endregion



        #region "Personas"

        public static List<Persona> LeerPersonas()
        {
            if (!realmInicializado) { throw new ErrorInicioSesionException(); }
            listaPersonas = [.. realm!.All<Persona>()];
            return listaPersonas;
            //return [.. _coleccionPersonas.AsQueryable().Where(p => p.Nombre != "")];
        }

        public static List<Persona> BuscarPersona(string nombre)
        {
            if (!realmInicializado) { throw new ErrorInicioSesionException(); }
            listaPersonas = listaPersonas.Where(p => p.Nombre!.Contains(nombre, StringComparison.CurrentCultureIgnoreCase)).ToList();
            return listaPersonas;
            //return [.. _coleccionPersonas.AsQueryable().Where(p => p.Nombre!.Contains(nombre, StringComparison.CurrentCultureIgnoreCase))];
        }

        public static async Task AgregarPersonaAsync(Persona persona)
        {
            if (!realmInicializado) { throw new ErrorInicioSesionException(); }
            await realm!.WriteAsync(() => { realm.Add(persona); });
            //await _coleccionPersonas!.InsertOneAsync(persona);
        }

        public static async Task BorrarPersona(ObjectId id)
        {
            //await _coleccionPersonas.DeleteOneAsync(p => p.Id == id);
            if (!realmInicializado) { throw new ErrorInicioSesionException(); }

            var temp = LeerPersonas();
            Persona personaBorrar = (Persona)temp.Select(p => p.Id == id);

            await realm!.WriteAsync(() => realm.Remove(personaBorrar));
        }

        public static async Task BorrarPersona(Persona persona)
        {
            //await BorrarPersona(persona.Id);
            if (!realmInicializado) { throw new ErrorInicioSesionException(); }
            await realm!.WriteAsync(() => realm.Remove(persona));
        }

        #endregion
    }
}

