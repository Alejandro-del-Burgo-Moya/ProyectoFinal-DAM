using MongoDB.Bson;
using ProyectoFinalDAM.Modelo.Enums;
using System.Security.Cryptography;

namespace ProyectoFinalDAM.Modelo
{
    public class Persona()
    {
        private ObjectId _id;
        private string? _nombre;
        private string? _apellido1;
        private string? _apellido2;
        private string? _nombreCompleto;
        private string? _contrasena;
        private string? _email;
        private Rol _rol;
        private Idioma? _idioma;
        private Tema? _tema;

        public ObjectId Id { get => _id; set => _id = value; }
        public string? Nombre { get => _nombre; set => _nombre = value; }
        public string? Apellido1 { get => _apellido1; set => _apellido1 = value; }
        public string? Apellido2 { get => _apellido2; set => _apellido2 = value; }
        public string? NombreCompleto { get => _nombreCompleto; set => _nombreCompleto = $"{_nombre} {_apellido1} {_apellido2}"; }
        public string? Contrasena { get => _contrasena; set => _contrasena = value; }
        public string? Email { get => _email; set => _email = value; }
        public Rol Rol { get => _rol; set => _rol = value; }
        public Idioma? Idioma { get => _idioma; set => _idioma = value; }
        public Tema? Tema { get => _tema; set => _tema = value; }
    }
}
