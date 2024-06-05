namespace ProyectoFinalDAM.Modelo.Excepciones
{
    [Serializable]
    internal class ErrorInicioSesionException : Exception
    {
        public ErrorInicioSesionException()
        {
        }

        public ErrorInicioSesionException(string? message) : base(message)
        {
        }

        public ErrorInicioSesionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
