using MongoDB.Bson;
using ProyectoFinalDAM.Modelo.Enums;
using Realms;

namespace ProyectoFinalDAM.Modelo
{
    public partial class Incidencia() : IRealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }
        public string? Nombre { get; set; }
        public string? Decripcion { get; set; }
        public int Prioridad { get; set; }
        public int Estado { get; set; }
        public DateTimeOffset FCreacion { get; set; }
        public DateTimeOffset? FAsignacion { get; set; }
        public DateTimeOffset? FResolucion { get; set; }
        public Persona? Creada { get; set; }
        public Persona? Asignada { get; set; }
        public Persona? Resuelta { get; set; }
    }
}
