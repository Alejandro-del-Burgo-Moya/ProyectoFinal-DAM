using MongoDB.Bson;

namespace ProyectoFinalDAM.Modelo
{
    public partial class Persona(string nombre, string apellido1, string? apellido2, string email, string contrasena, int rol)
    {
        public ObjectId Id { get; set; }
        public string Nombre { get; set; } = nombre;
        public string Apellido1 { get; set; } = apellido1;
        public string? Apellido2 { get; set; } = apellido2;
        public string? NombreCompleto { get => $"{Nombre} {Apellido1} {Apellido2}"; }
        public string Contrasena { get; set; } = contrasena;
        public string Email { get; set; } = email;
        public int Rol { get; set; } = rol;

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

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            try
            {
                Persona persona = (Persona)obj;
                return Id.Equals(persona.Id);
            }
            catch (Exception) { return false; }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, Apellido1, Apellido2, NombreCompleto);
        }
    }
}
