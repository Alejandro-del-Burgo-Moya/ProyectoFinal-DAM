using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Enums;

namespace ProyectoFinalDAM.Vista.Modificar;

public partial class VistaModificarIncidencia : ContentPage
{
	private readonly AppShell _appShell;
	private readonly Incidencia incidencia;

	public VistaModificarIncidencia(AppShell appShell, Incidencia incidencia)
	{
		InitializeComponent();

		_appShell = appShell;
		this.incidencia = incidencia;

        PickerPrioridad.ItemsSource = null;
        PickerPrioridad.ItemsSource = Utiles.NombresPrioridad();
        PickerPrioridad.SelectedIndex = 0;

        TxtIncidencia.Text = this.incidencia.Nombre;
        TxtDescripcion.Text = this.incidencia.Descripcion;
        PickerPrioridad.SelectedIndex = this.incidencia.Prioridad;
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
        incidencia.Descripcion = TxtDescripcion.Text;
        incidencia.Prioridad = PickerPrioridad.SelectedIndex;

        //incidencia.Creada = usuarioLogeado    //TODO manejar el usuario logeadop en la aplicación

        _appShell.ModificarIncidencia(incidencia);

        Navigation.PopModalAsync(true);
    }
}