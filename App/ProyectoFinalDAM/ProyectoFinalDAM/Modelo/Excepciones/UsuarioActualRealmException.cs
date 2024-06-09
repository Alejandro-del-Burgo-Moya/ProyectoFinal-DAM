namespace ProyectoFinalDAM.Modelo.Excepciones
{
    [Serializable]
    internal class UsuarioActualRealmException : Exception
    {
        public UsuarioActualRealmException()
        {
        }

        public UsuarioActualRealmException(string? message) : base(message)
        {
        }

        public UsuarioActualRealmException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}