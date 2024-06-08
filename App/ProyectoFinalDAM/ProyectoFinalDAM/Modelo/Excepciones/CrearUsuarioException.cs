namespace ProyectoFinalDAM.Modelo.Excepciones
{
    [Serializable]
    internal class CrearUsuarioException : Exception
    {
        public CrearUsuarioException()
        {
        }

        public CrearUsuarioException(string? message) : base(message)
        {
        }

        public CrearUsuarioException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}