using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Vista.Modificar;

namespace ProyectoFinalDAM.Vista;

public partial class VistaIncidencias : ContentPage
{
    private readonly AppShell _appShell;
    private int? estado = null;
    private int? prioridad = null;
    private int? orden = null;

    public VistaIncidencias(AppShell appShell)
    {
        InitializeComponent();

        _appShell = appShell;

        InicializarPickers();
    }


    private void InicializarPickers()
    {
        List<string> filtroOrden =
        [
            "Más recientes primero",
            "Más antiguas primero",
            "Más prioritarias primero",
            "Menos prioritarias primero",
            "Más avanzadas primero",
            "Menos avanzadas primero",
        ];
        PickerOrden.ItemsSource = filtroOrden;
        PickerOrden.SelectedIndex = 0;

        InicializarPickerEstado();
        InicializarPickerPrioridad();
    }


    private void InicializarPickerEstado()
    {
        List<string> filtroEstado = Utiles.NombresEstado();
        PickerFiltroEstado.ItemsSource = null;
        PickerFiltroEstado.ItemsSource = filtroEstado;
    }


    private void InicializarPickerPrioridad()
    {
        List<string> filtroPrioridad = Utiles.NombresPrioridad();
        PickerFiltroPrioridad.ItemsSource = null;
        PickerFiltroPrioridad.ItemsSource = filtroPrioridad;
    }


    private void RellenarListaIncidencias()
    {
        var lista = _appShell.LeerIncidencias(estado, prioridad, orden, TxtBuscar.Text);
        ListaIncidencias.ItemsSource = null;
        ListaIncidencias.ItemsSource = lista;
    }


    private void PickerFiltroEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        estado = PickerFiltroEstado.SelectedIndex;
        RellenarListaIncidencias();
    }


    private void PickerFiltroPrioridad_SelectedIndexChanged(object sender, EventArgs e)
    {
        prioridad = PickerFiltroPrioridad.SelectedIndex;
        RellenarListaIncidencias();
    }


    private void PickerOrden_SelectedIndexChanged(object sender, EventArgs e)
    {
        orden = PickerOrden.SelectedIndex;
        RellenarListaIncidencias();
    }


    private void BtnBorrarFiltros_Clicked(object sender, EventArgs e)
    {
        InicializarPickerEstado();
        InicializarPickerPrioridad();
        PickerOrden.SelectedIndex = 0;
        TxtBuscar.Text = null;
        RellenarListaIncidencias();
    }


    private void BtnCrearIncidencia_Clicked(object sender, EventArgs e)
    {
        _ = Navigation.PushModalAsync(new VistaCrearIncidencia(_appShell), true);
        RellenarListaIncidencias();
    }


    private void Buscar_Clicked(object sender, EventArgs e)
    {
        RellenarListaIncidencias();
    }


    private void ActualizarListaIncidencias_Clicked(object sender, EventArgs e)
    {
        InicializarPickerEstado();
        InicializarPickerPrioridad();
        PickerOrden.SelectedIndex = 0;
        TxtBuscar.Text = null;

        RellenarListaIncidencias();
    }


    private void BtnModificarIncidencia_Clicked(object sender, EventArgs e)
    {
        if (ListaIncidencias.SelectedItem != null)
        {
            Incidencia incidencia = (Incidencia)ListaIncidencias.SelectedItem;
            _ = Navigation.PushModalAsync(new VistaModificarIncidencia(_appShell, incidencia));
        }
        else
        {
            Utiles.MostrarAdvertencia("Error", "Debes seleccionar una incidencia de la lista");
        }
    }
}