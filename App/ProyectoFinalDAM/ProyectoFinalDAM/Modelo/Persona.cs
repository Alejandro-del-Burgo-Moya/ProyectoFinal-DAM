using Microsoft.Maui.ApplicationModel.Communication;
using MongoDB.Bson;
using ProyectoFinalDAM.Modelo.Enums;
using Realms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProyectoFinalDAM.Modelo
{
    public partial class Persona : IRealmObject
    {
        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string? Apellido2 { get; set; }
        public string? NombreCompleto { get => $"{Nombre} {Apellido1} {Apellido2}"; }
        public string Contrasena { get; set; }
        public string Email { get; set; }
        public int Rol { get; set; }

        public Persona()
        {
            Nombre = "";
            Apellido1 = "";
            Contrasena = "";
            Email = "";
            Rol = 0;
        }
        public Persona(string nombre, string apellido1, string contrasena, string email, int rol)
        {
            Nombre = nombre;
            Apellido1 = apellido1;
            Contrasena = contrasena;
            Email = email;
            Rol = rol;
        }
    }
}
