using MongoDB.Driver;
using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Enums;
using ProyectoFinalDAM.Modelo.Excepciones;

namespace ProyectoFinalDAM.BaseDatos
{
    public class Mongo
    {
        //mongodb+srv://adburgom01:b8nl7320c@proyectofinaldam.q49ehh3.mongodb.net/?retryWrites=true&w=majority&appName=ProyectoFinalDAM
        //private readonly string URIConexion = "mongodb+srv://adburgom01:b8nl7320c@proyectofinaldam.q49ehh3.mongodb.net/?retryWrites=true&w=majority&appName=ProyectoFinalDAM";
        //mongodb+srv://adburgom01:b8nl7320c@proyectofinaldam.q49ehh3.mongodb.net/test?retryWrites=true&w=majority
        private readonly string nombreBaseDatos = "ProyectoFinalDAM";
        private readonly string coleccionIncidencias = "Incidencias";
        private readonly string coleccionPersonas = "Personas";
        private readonly string URIConexion = "mongodb://adburgom01:b8nl7320c@proyectofinaldam.q49ehh3.mongodb.net/?retryWrites=true&w=majority&appName=ProyectoFinalDAM";
        private readonly MongoClient _client;
        private readonly IMongoCollection<Incidencia> _coleccionIncidencias;
        private readonly IMongoCollection<Persona> _coleccionPersonas;

        public Mongo()
        {
            var settings = MongoClientSettings.FromConnectionString(URIConexion);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            _client = new MongoClient(settings);
            _coleccionIncidencias = _client.GetDatabase(nombreBaseDatos).GetCollection<Incidencia>(coleccionIncidencias);
            _coleccionPersonas = _client.GetDatabase(nombreBaseDatos).GetCollection<Persona>(coleccionPersonas);
        }

        #region "Incidencias"

        public async Task<List<Incidencia>> LeerIncidenciasAsync()
        {
            return await _coleccionIncidencias.AsQueryable().ToListAsync<Incidencia>();
        }

        public List<Incidencia> LeerIncidenciasFiltroEstado(Estado estado)
        {
            return [.. _coleccionIncidencias.AsQueryable().Where(i => i.Estado == estado)];
        }

        public List<Incidencia> LeerIncidenciasFiltroPrioridad(Prioridad prioridad)
        {
            return [.. _coleccionIncidencias.AsQueryable().Where(i => i.Prioridad == prioridad)];
        }

        public List<Incidencia> LeerIncidenciasOrden(int orden)
        {
            return orden switch
            {
                0 => [.. _coleccionIncidencias.AsQueryable().OrderBy(i => i.FCreacion)],
                1 => [.. _coleccionIncidencias.AsQueryable().OrderBy(i => i.FCreacion).Reverse()],
                2 => [.. _coleccionIncidencias.AsQueryable().OrderBy(i => i.Prioridad)],
                3 => [.. _coleccionIncidencias.AsQueryable().OrderBy(i => i.Prioridad).Reverse()],
                4 => [.. _coleccionIncidencias.AsQueryable().OrderBy(i => i.Estado)],
                5 => [.. _coleccionIncidencias.AsQueryable().OrderBy(i => i.Estado).Reverse()],
                _ => throw new OrdenIncidenciasException("Se ha introducido un orden no controlado => " + orden),
            };
        }

        public async Task CrearIncidenciaAsync(Incidencia incidencia)
        {
            await _coleccionIncidencias.InsertOneAsync(incidencia);
        }

        #endregion

        #region "Personas"

        public async Task<List<Persona>> LeerPersoansAsyn()
        {
            return await _coleccionPersonas.AsQueryable().ToListAsync<Persona>();
        }

        public async Task AgregarPersonaAsync(Persona persona)
        {
            await _coleccionPersonas.InsertOneAsync(persona);
        }

        #endregion
    }
}

