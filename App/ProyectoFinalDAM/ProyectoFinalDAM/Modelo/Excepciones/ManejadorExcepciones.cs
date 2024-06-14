namespace ProyectoFinalDAM.Modelo.Excepciones
{
    public static class ManejadorExcepciones
    {
        public static void Manejar(Exception ex)
        {
            //System.Diagnostics.Debug.WriteLine(ex.ToString());
            Utiles.MostrarAdvertencia($"Excepción: {ex.GetType()}", $"Mensaje:\n\t{ex.Message}\nOrigen:\n\t{ex.Source}\nTraza:\n{ex.StackTrace}");
        }
    }
}
