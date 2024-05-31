using ProyectoFinalDAM.BaseDatos;
using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Enums;

namespace ProyectoFinalDAM.Vista;

public partial class VistaIncidencias : ContentPage
{
    private readonly Mongo mongo;

    public VistaIncidencias(Mongo mongo)
    {
        InitializeComponent();

        InicializarPickers();
        this.mongo = mongo;
    }

    private void InicializarPickers()
    {
        List<string> filtroOrden =
        [
            "M�s recientes primero",
            "M�s antiguas priemro",
            "M�s prioritarias primero",
            "Menos prioritarias primero",
            "Menos avanzadas primero",
            "M�s avanzadas primero",
        ];
        PickerOrden.ItemsSource = filtroOrden;
        PickerOrden.SelectedIndex = 0;

        InicializarPickerEstado();
        InicializarPickerPrioridad();
    }

    private void InicializarPickerEstado()
    {
        List<string> filtroEstado = [.. Enum.GetNames<Estado>()];
        PickerFiltroEstado.ItemsSource = null;
        PickerFiltroEstado.ItemsSource = filtroEstado;
    }

    private void InicializarPickerPrioridad()
    {
        List<string> filtroPrioridad = [.. Enum.GetNames<Prioridad>()];
        PickerFiltroPrioridad.ItemsSource = null;
        PickerFiltroPrioridad.ItemsSource = filtroPrioridad;
    }

    private void PickerFiltroEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PickerFiltroEstado.SelectedIndex == Enum.GetNames<Estado>().Length)
        {
            PickerFiltroEstado.ItemsSource = null;
            InicializarPickerEstado();
        }
        else
        {
            //TODO: llamada al m�todo de la base de datos
        }
    }

    private void PickerFiltroPrioridad_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PickerFiltroPrioridad.SelectedIndex == Enum.GetNames<Prioridad>().Length)
        {
            InicializarPickerPrioridad();
        }
        else
        {
            //TODO: llamada al m�todo de la base de datos
        }
    }

    private void PickerOrden_SelectedIndexChanged(object sender, EventArgs e)
    {
        //TODO: llamada al m�todo de la base de datos
    }

    private void BtnBorrarFiltros_Clicked(object sender, EventArgs e)
    {
        InicializarPickerEstado();
        InicializarPickerPrioridad();
    }

    private void BtnCrearIncidencia_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new VistaCrearIncidencia(mongo), true);
        _ = mongo.CrearIncidenciaAsync(new Incidencia());
    }
}