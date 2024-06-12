using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Enums;

namespace ProyectoFinalDAM.Vista;

public partial class VistaCrearIncidencia : ContentPage
{
    private readonly AppShell _appShell;

    public VistaCrearIncidencia(AppShell appShell)
    {
        InitializeComponent();

        _appShell = appShell;

        PickerPrioridad.ItemsSource = null;
        PickerPrioridad.ItemsSource = Utiles.NombresPrioridad();
        PickerPrioridad.SelectedIndex = 0;
    }

    private void BtnBorrarCampos_Clicked(object sender, EventArgs e)
    {
        TxtIncidencia.Text = null;
        TxtDescripcion.Text = null;
        PickerPrioridad.SelectedIndex = 0;
    }

    private void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync(true);
    }

    private void BtnCrearIncidencia_Clicked(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(TxtIncidencia.Text))
        {
            Incidencia incidencia = new()
            {
                Nombre = TxtIncidencia.Text,
                Descripcion = TxtDescripcion.Text,
                Estado = (int)Estado.Abierta,
                Prioridad = PickerPrioridad.SelectedIndex,
                FCreacion = DateTime.Now
            };
            //incidencia.Creada = usuarioLogeado    //TODO manejar el usuario logeadop en la aplicación

            _appShell.CrearIncidencia(incidencia);

            Navigation.PopModalAsync(true);
        }
        else
        {
            Utiles.MostrarAdvertencia(Utiles.ExtraerValorDiccionario("falta_nombre"), Utiles.ExtraerValorDiccionario("falta_nombre_desc"));
        }
    }
}