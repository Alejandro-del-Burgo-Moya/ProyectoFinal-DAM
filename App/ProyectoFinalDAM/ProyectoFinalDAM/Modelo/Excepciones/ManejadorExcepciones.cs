namespace ProyectoFinalDAM.Modelo.Excepciones
{
    public static class ManejadorExcepciones
    {
        public static void Manejar(Exception ex)
        {
            Application.Current!.MainPage!.DisplayAlert(
                $"Excepción: {ex.GetType()}", 
                $"Mensaje:\n\t{ex.Message}\nOrigen:\n\t{ex.Source}\nTraza:\n{ex.StackTrace}", 
                "OK");
        }
    }
}
