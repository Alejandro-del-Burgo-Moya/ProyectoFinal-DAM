using MongoDB.Driver;
using ProyectoFinalDAM.Modelo;

namespace ProyectoFinalDAM.BaseDatos
{
    public class Mongo()
    {
        //TODO: borrar antes de subir
        private static readonly string Uri = "mongodb://adburgom01:b8nl7320c@ac-yjqnrx6-shard-00-00.q49ehh3.mongodb.net:27017,ac-yjqnrx6-shard-00-01.q49ehh3.mongodb.net:27017,ac-yjqnrx6-shard-00-02.q49ehh3.mongodb.net:27017/?ssl=true&replicaSet=atlas-vs2ruz-shard-0&authSource=admin&retryWrites=true&w=majority&appName=ProyectoFinalDAM";
        private static readonly MongoClientSettings ajustes = MongoClientSettings.FromConnectionString(Uri);
        private static readonly MongoClient cliente = new(ajustes);
        private IMongoCollection<Persona> colPersonas = cliente.GetDatabase("ProyectoFinalDAM").GetCollection<Persona>("Persona");
        private IMongoCollection<Incidencia> colIncidencias = cliente.GetDatabase("ProyectoFinalDAM").GetCollection<Incidencia>("Incidencia");

        public bool IniciarSesion(string email, string contrasena)
        {
            return LeerPersonas().Where(p => p.Email == email && p.Contrasena == contrasena).Any();
        }

        public void RegistrarUsuario(Persona persona)
        {
            CrearPersona(persona);
        }

        public List<Persona> LeerPersonas()
        {
            colPersonas = cliente.GetDatabase("ProyectoFinalDAM").GetCollection<Persona>("Persona");
            return colPersonas.Find(_ => true).ToList();
        }

        public List<Incidencia> LeerIncidencias()
        {
            colIncidencias = cliente.GetDatabase("ProyectoFinalDAM").GetCollection<Incidencia>("Incidencia");
            return colIncidencias.Find(_ => true).ToList();
        }

        public void CrearPersona(Persona persona)
        {
            colPersonas.InsertOne(persona);
        }

        public void CrearIncidencia(Incidencia incidencia)
        {
            colIncidencias.InsertOne(incidencia);
        }

        public void ModificarIncidencia(Incidencia incidencia)
        {
            var filtro = Builders<Incidencia>.Filter.Eq("Id", incidencia.Id);
            var cambios = Builders<Incidencia>.Update
                .Set("Descripcion", incidencia.Descripcion)
                .Set("Prioridad", incidencia.Prioridad);
            colIncidencias.UpdateOne(filtro, cambios);
        }

        public void ModificarPersona(Persona persona)
        {
            var filtro = Builders<Persona>.Filter.Eq("Id", persona.Id);
            var cambios = Builders<Persona>.Update
                .Set("Email", persona.Email)
                .Set("Contrasena", persona.Contrasena)
                .Set("Rol", persona.Rol);
            colPersonas.UpdateOne(filtro, cambios);
        }
    }
}

