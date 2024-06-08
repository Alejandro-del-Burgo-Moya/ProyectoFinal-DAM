using MongoDB.Bson;
using ProyectoFinalDAM.Modelo.Enums;
using Realms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProyectoFinalDAM.Modelo
{
    public partial class Incidencia : IRealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }
        public string Nombre { get; set; }
        public string? Decripcion { get; set; }
        public int Prioridad { get; set; }
        public int Estado { get; set; }
        public DateTimeOffset FCreacion { get; set; }
        public DateTimeOffset? FAsignacion { get; set; }
        public DateTimeOffset? FResolucion { get; set; }
        public Persona? Creada { get; set; }
        public Persona? Asignada { get; set; }
        public Persona? Resuelta { get; set; }

        public Incidencia()
        {
            Nombre = "";
            Prioridad = -1;
            Estado = -1;
            FCreacion = DateTimeOffset.MinValue;
        }
        public Incidencia(string nombre, int prioridad, int estado, DateTimeOffset fCreacion)
        {
            Nombre = nombre;
            Prioridad = prioridad;
            Estado = estado;
            FCreacion = fCreacion;
        }
    }
}
