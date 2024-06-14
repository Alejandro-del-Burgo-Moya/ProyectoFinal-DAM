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

        public Persona? IniciarSesion(string email, string contrasena)
        {
            try
            {
                //return LeerPersonas().Where(p => p.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase) && p.Contrasena.Equals(contrasena)).First();
                var lista = LeerPersonas();
                lista = lista.Where(p => p.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase) && p.Contrasena.Equals(contrasena)).ToList();
                return lista.First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="persona"></param>
        /// <returns>Devuelve el rol de la persona que se ha registrado (2 -> admin, 1 -> tecnico, 0 -> normal, -1 -> ya existe ese email)</returns>
        public int RegistrarUsuario(Persona persona)
        {
            //Si es el pirmie usuario se crea como admin
            var lista = LeerPersonas();
            bool hayAdmin = lista.Where(p => p.Rol == 2).Any();
            if (!hayAdmin) persona.Rol = 2;

            //Si ya existe ese email no se puede crear un usuario
            bool yaExiste = lista.Where(p => p.Email.Equals(persona.Email, StringComparison.CurrentCultureIgnoreCase)).Any();
            if (yaExiste) return -1;

            CrearPersona(persona);
            return persona.Rol;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="incidencia"></param>
        /// <returns>Devuelve true si se modifica la incidencia y false si la incidencia está resulta</returns>
        public bool ModificarIncidencia(Incidencia incidencia)
        {
            if (incidencia.Estado == 3) { return false; }
            var filtro = Builders<Incidencia>.Filter.Eq("Id", incidencia.Id);
            var cambios = Builders<Incidencia>.Update
                .Set("Descripcion", incidencia.Descripcion)
                .Set("Prioridad", incidencia.Prioridad);
            colIncidencias.UpdateOne(filtro, cambios);
            return true;
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

        public void BorrarUsuario(Persona persona)
        {
            var filtro = Builders<Persona>.Filter.Eq("Id", persona.Id);
            colPersonas.DeleteOne(filtro);
        }

        public void AsignarIncidencia(Incidencia incidencia, Persona persona)
        {
            incidencia.Asignada = persona;
            incidencia.FAsignacion = DateTime.Now;
            incidencia.Estado = 1;

            var filtro = Builders<Incidencia>.Filter.Eq("Id", incidencia.Id);
            var cambios = Builders<Incidencia>.Update
                .Set("Asignada", incidencia.Asignada)
                .Set("FAsignacion", incidencia.FAsignacion)
                .Set("Estado", incidencia.Estado);
            colIncidencias.UpdateOne(filtro, cambios);
        }

        public void ResolverIncidencia(Incidencia incidencia, Persona persona)
        {
            incidencia.Resuelta = persona;
            incidencia.FResolucion = DateTime.Now;
            incidencia.Estado = 3;

            var filtro = Builders<Incidencia>.Filter.Eq("Id", incidencia.Id);
            var cambios = Builders<Incidencia>.Update
                .Set("Asignada", incidencia.Asignada)
                .Set("FAsignacion", incidencia.FAsignacion)
                .Set("Estado", incidencia.Estado);
            colIncidencias.UpdateOne(filtro, cambios);
        }
    }
}

