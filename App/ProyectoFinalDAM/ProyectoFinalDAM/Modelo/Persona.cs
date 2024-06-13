using MongoDB.Bson;

namespace ProyectoFinalDAM.Modelo
{
    public partial class Persona
    {
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

        public override string ToString()
        {
            string nombreCompletoPersona = Utiles.ExtraerValorDiccionario("nombre_per");
            string emailPersona = Utiles.ExtraerValorDiccionario("email_per");
            string textoRol = Utiles.ExtraerValorDiccionario("rol_per");
            string rolPersona = Rol switch
            {
                0 => Utiles.ExtraerValorDiccionario("rol_normal"),
                1 => Utiles.ExtraerValorDiccionario("rol_tecnico"),
                2 => Utiles.ExtraerValorDiccionario("rol_admin"),
                _ => "error",
            };
            return 
                $"{nombreCompletoPersona}: {NombreCompleto}\n" +
                $"{emailPersona}: {Email}\n" +
                $"{textoRol}: {rolPersona}\n";
        }
    }
}
