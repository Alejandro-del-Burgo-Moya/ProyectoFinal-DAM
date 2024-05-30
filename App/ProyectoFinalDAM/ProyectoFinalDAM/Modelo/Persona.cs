using ProyectoFinalDAM.Modelo.Enums;

namespace ProyectoFinalDAM.Modelo
{
    public class Persona(string nombre, string contrasena, Rol rol)
    {
        private string _nombre = nombre;
        private string _contrasena = contrasena;
        private Rol _rol = rol;

        public string Nombre { get => _nombre; set => _nombre = value; }
        public string Contrasena { get => _contrasena; set => _contrasena = value; }
        public Rol Rol { get => _rol; set => _rol = value; }
    }
}
