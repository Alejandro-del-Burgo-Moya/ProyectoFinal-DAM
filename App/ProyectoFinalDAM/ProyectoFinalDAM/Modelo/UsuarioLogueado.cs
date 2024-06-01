namespace ProyectoFinalDAM.Modelo
{
    public static class UsuarioLogueado
    {
        private static Persona? _usuario;
        public static Persona? Usuario  { get => _usuario; set => _usuario = value; }
    }
}
