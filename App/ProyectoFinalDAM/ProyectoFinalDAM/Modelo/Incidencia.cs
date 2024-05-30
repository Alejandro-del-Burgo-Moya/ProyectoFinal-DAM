using ProyectoFinalDAM.Modelo.Enums;

namespace ProyectoFinalDAM.Modelo
{
    public class Incidencia()
    {
        private string? _nombre;
        private string? _decripcion;
        private Prioridad _prioridad;
        private Estado _estado;
        private DateTime _fCreacion;
        private DateTime? _fAsignacion;
        private DateTime? _fResolucion;
        private Persona? _creada;
        private Persona? _asignada;
        private Persona? _resuelta;

        public string? Nombre { get => _nombre; set => _nombre = value; }
        public string? Decripcion { get => _decripcion; set => _decripcion = value; }
        public Prioridad Prioridad { get => _prioridad; set => _prioridad = value; }
        public Estado Estado { get => _estado; set => _estado = value; }
        public DateTime FCreacion { get => _fCreacion; set => _fCreacion = value; }
        public DateTime? FAsignacion { get => _fAsignacion; set => _fAsignacion = value; }
        public DateTime? FResolucion { get => _fResolucion; set => _fResolucion = value; }
        public Persona? Creada { get => _creada; set => _creada = value; }
        public Persona? Asignada { get => _asignada; set => _asignada = value; }
        public Persona? Resuelta { get => _resuelta; set => _resuelta = value; }
    }
}
