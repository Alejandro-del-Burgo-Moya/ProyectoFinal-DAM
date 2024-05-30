namespace ProyectoFinalDAM.Modelo.Excepciones
{
    [Serializable]
    internal class OrdenIncidenciasException : Exception
    {
        public OrdenIncidenciasException()
        {
        }

        public OrdenIncidenciasException(string? message) : base(message)
        {
        }

        public OrdenIncidenciasException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}