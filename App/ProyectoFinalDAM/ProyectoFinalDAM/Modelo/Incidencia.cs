using MongoDB.Bson;
using Realms;

namespace ProyectoFinalDAM.Modelo
{
    public partial class Incidencia
    {
        public ObjectId Id { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int Prioridad { get; set; }
        public int Estado { get; set; }
        public DateTimeOffset FCreacion { get; set; }
        public DateTimeOffset? FAsignacion { get; set; }
        public DateTimeOffset? FResolucion { get; set; }
        public Persona? Creada { get; set; }
        public Persona? Asignada { get; set; }
        public Persona? Resuelta { get; set; }

        public Incidencia()
        {
            Nombre = "";
            Prioridad = -1;
            Estado = -1;
            FCreacion = DateTimeOffset.MinValue;
        }
        public Incidencia(string nombre, int prioridad, int estado, DateTimeOffset fCreacion)
        {
            Nombre = nombre;
            Prioridad = prioridad;
            Estado = estado;
            FCreacion = fCreacion;
        }

        public override string ToString()
        {
            string nombreInc = Utiles.ExtraerValorDiccionario("nombre_incidencia");
            string estadoInc = Utiles.ExtraerValorDiccionario("estado_incidencia");
            string prioridadInc = Utiles.ExtraerValorDiccionario("prioridad_incidencia");
            string valorEstado = Estado switch
            {
                0 => Utiles.ExtraerValorDiccionario("estado_abierta"),
                1 => Utiles.ExtraerValorDiccionario("estado_asignada"),
                2 => Utiles.ExtraerValorDiccionario("estado_en_progreso"),
                3 => Utiles.ExtraerValorDiccionario("estado_resuelta"),
                _ => "error",
            };
            string valorPrioridad = Prioridad switch
            {
                0 => Utiles.ExtraerValorDiccionario("prioridad_baja"),
                1 => Utiles.ExtraerValorDiccionario("prioridad_media"),
                2 => Utiles.ExtraerValorDiccionario("prioridad_alta"),
                _ => "error",
            };

            return $"{nombreInc}: {Nombre}\n{estadoInc}: {valorEstado}\n{prioridadInc}: {valorPrioridad}\n";
        }
    }
}
