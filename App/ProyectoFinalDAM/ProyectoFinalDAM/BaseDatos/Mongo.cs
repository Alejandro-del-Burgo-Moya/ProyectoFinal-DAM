using MongoDB.Bson;
using MongoDB.Driver;
using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Enums;
using ProyectoFinalDAM.Modelo.Excepciones;

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
        private static MongoClient? _client = null;
        private static IMongoCollection<Incidencia>? _coleccionIncidencias;
        private static IMongoCollection<Persona>? _coleccionPersonas;

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
            var settings = MongoClientSettings.FromConnectionString(URIConexion);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            _client = new MongoClient(settings);
            _coleccionIncidencias = _client.GetDatabase(nombreBaseDatos).GetCollection<Incidencia>(coleccionIncidencias);
            _coleccionPersonas = _client.GetDatabase(nombreBaseDatos).GetCollection<Persona>(coleccionPersonas);
        }



        #region "Incidencias"

        public static List<Incidencia> LeerIncidencias()
        {
            if (_client == null) { InicializarMongo(); }
            return [.. _coleccionIncidencias.AsQueryable().Where(i => i.Nombre != "")];
        }

        public static List<Incidencia> LeerIncidenciasFiltroEstado(Estado estado)
        {
            if (_client == null) { InicializarMongo(); }
            return [.. _coleccionIncidencias.AsQueryable().Where(i => i.Estado == estado)];
        }

        public static List<Incidencia> LeerIncidenciasFiltroPrioridad(Prioridad prioridad)
        {
            if (_client == null) { InicializarMongo(); }
            return [.. _coleccionIncidencias.AsQueryable().Where(i => i.Prioridad == prioridad)];
        }

        public static List<Incidencia> LeerIncidenciasOrden(int orden)
        {
            if (_client == null) { InicializarMongo(); }
            return orden switch
            {
                0 => [.. _coleccionIncidencias.AsQueryable().OrderBy(i => i.FCreacion)],
                1 => [.. _coleccionIncidencias.AsQueryable().OrderByDescending(i => i.FCreacion)],
                2 => [.. _coleccionIncidencias.AsQueryable().OrderByDescending(i => i.Prioridad)],
                3 => [.. _coleccionIncidencias.AsQueryable().OrderBy(i => i.Prioridad)],
                4 => [.. _coleccionIncidencias.AsQueryable().OrderByDescending(i => i.Estado)],
                5 => [.. _coleccionIncidencias.AsQueryable().OrderBy(i => i.Estado)],
                _ => throw new OrdenIncidenciasException("Se ha introducido un orden no controlado => " + orden),
            };
        }

        public static async Task CrearIncidenciaAsync(Incidencia incidencia)
        {
            await _coleccionIncidencias!.InsertOneAsync(incidencia);
        }

        public static List<Incidencia> BuscarIncidencia(string nombre)
        {
            return [.. _coleccionIncidencias.AsQueryable().Where(i => i.Nombre!.Contains(nombre, StringComparison.CurrentCultureIgnoreCase))];
        }

        public static async Task BorrarIncidencia(ObjectId id)
        {
            await _coleccionIncidencias.DeleteOneAsync(i => i.Id == id);
        }

        public static async Task BorrarIncidencia(Incidencia incidencia)
        {
            await BorrarIncidencia(incidencia.Id);
        }

        public static async Task AsignarIncidencia(Incidencia incidencia, Persona persona)
        {
            var filtro = Builders<Incidencia>.Filter.Eq(i => i.Id, incidencia.Id);
            var actualizacion = Builders<Incidencia>.Update
                .Set(i => i.Asignada, persona)
                .Set(i => i.FAsignacion, DateTime.Now)
                .Set(i => i.Estado, Estado.Asignada);
            await _coleccionIncidencias!.UpdateOneAsync(filtro, actualizacion);
        }

        public static async Task ResolverIncidenciaAdmin(Incidencia incidencia)
        {
            var filtro = Builders<Incidencia>.Filter.Eq(i => i.Id, incidencia.Id);
            var actualizacion = Builders<Incidencia>.Update
                .Set(i => i.Resuelta, incidencia.Asignada)
                .Set(i => i.FResolucion, DateTime.Now)
                .Set(i => i.Estado, Estado.Resuelta);
            await _coleccionIncidencias!.UpdateOneAsync(filtro, actualizacion);
        }

        #endregion



        #region "Personas"

        public static List<Persona> LeerPersonas()
        {
            if (_client == null) { InicializarMongo(); }
            return [.. _coleccionPersonas.AsQueryable().Where(p => p.Nombre != "")];
        }

        public static async Task AgregarPersonaAsync(Persona persona)
        {
            await _coleccionPersonas!.InsertOneAsync(persona);
        }

        public static List<Persona> BuscarPersona(string nombre)
        {
            return [.. _coleccionPersonas.AsQueryable().Where(p => p.Nombre!.Contains(nombre, StringComparison.CurrentCultureIgnoreCase))];
        }

        public static async Task BorrarPersona(ObjectId id)
        {
            await _coleccionPersonas.DeleteOneAsync(p => p.Id == id);
        }

        public static async Task BorrarPersona(Persona persona)
        {
            await BorrarPersona(persona.Id);
        }

        #endregion
    }
}

