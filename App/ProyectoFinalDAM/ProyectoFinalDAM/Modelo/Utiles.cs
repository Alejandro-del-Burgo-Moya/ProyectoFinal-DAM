namespace ProyectoFinalDAM.Modelo
{
    public static class Utiles
    {
        public static string ExtraerValorDiccionario(string clave)
        {
            if (App.Current != null)
            {
                return App.Current.Resources.TryGetValue(clave, out object valor) ? (string)valor : "error";
            }
            else { return "error"; }
        }

        public static void MostrarAdvertencia(string titulo, string mensaje)
        {
            if (App.Current != null && App.Current.MainPage != null)
            {
                Application.Current.MainPage.DisplayAlert(titulo, mensaje, "OK");
            }
        }

        public static List<string> NombresEstado()
        {
            List<string> estados =
            [
                ExtraerValorDiccionario("estado_abierta"),
                ExtraerValorDiccionario("estado_asignada"),
                ExtraerValorDiccionario("estado_en_progreso"),
                ExtraerValorDiccionario("estado_resuelta"),
            ];
            return estados;
        }

        public static List<string> NombresPrioridad()
        {
            List<string> prioridades =
            [
                ExtraerValorDiccionario("prioridad_baja"),
                ExtraerValorDiccionario("prioridad_media"),
                ExtraerValorDiccionario("prioridad_alta"),
            ];

            return prioridades;
        }
    }
}
