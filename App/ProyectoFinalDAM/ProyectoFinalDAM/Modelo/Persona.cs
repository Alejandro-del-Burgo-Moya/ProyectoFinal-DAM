using MongoDB.Bson;
using ProyectoFinalDAM.Modelo.Enums;
using Realms;

namespace ProyectoFinalDAM.Modelo
{
    public partial class Persona() : IRealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido1 { get; set; }
        public string? Apellido2 { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Contrasena { get; set; }
        public string? Email { get; set; }
        public int Rol { get; set; }
    }
}
