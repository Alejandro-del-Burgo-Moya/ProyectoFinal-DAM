using ProyectoFinalDAM.Modelo;

namespace ProyectoFinalDAM.Vista.Modificar;

public partial class VistaModificarIncidencia : ContentPage
{
    private readonly AppShell _appShell;
    private readonly Incidencia _incidencia;

    public VistaModificarIncidencia(AppShell appShell, Incidencia incidencia)
    {
        InitializeComponent();

        _appShell = appShell;
        _incidencia = incidencia;

        InicializarPickerPrioridad();

        TxtIncidencia.Text = _incidencia.Nombre;
        TxtDescripcion.Text = _incidencia.Descripcion;
        PickerPrioridad.SelectedIndex = _incidencia.Prioridad;
        TxtEstadoincidencia.Text = Utiles.NombresEstado()[_incidencia.Estado];
    }

    private void InicializarPickerPrioridad()
    {
        PickerPrioridad.ItemsSource = null;
        PickerPrioridad.ItemsSource = Utiles.NombresPrioridad();
        PickerPrioridad.SelectedIndex = 0;
    }

    private void BtnBorrarCampos_Clicked(object sender, EventArgs e)
    {
        TxtDescripcion.Text = null;
        PickerPrioridad.SelectedIndex = 0;
    }

    private void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync(true);
    }

    private void BtnGuardarCambiosIncidencia_Clicked(object sender, EventArgs e)
    {
        _incidencia.Descripcion = TxtDescripcion.Text;
        _incidencia.Prioridad = PickerPrioridad.SelectedIndex;

        _appShell.ModificarIncidencia(_incidencia);


        Navigation.PopModalAsync(true);
    }
}