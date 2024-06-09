using ProyectoFinalDAM.BaseDatos;
using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Enums;

namespace ProyectoFinalDAM.Vista;

public partial class VistaCrearIncidencia : ContentPage
{
    private readonly AppShell _appShell;
    private readonly Incidencia incidencia;

    public VistaCrearIncidencia(AppShell appShell)
    {
        InitializeComponent();

        _appShell = appShell;

        this.incidencia = new();

        List<string> Prioridades = [.. Enum.GetNames<Prioridad>()];
        PickerPrioridad.ItemsSource = null;
        PickerPrioridad.ItemsSource = Prioridades;
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
            incidencia.Nombre = TxtIncidencia.Text;
            incidencia.Decripcion = TxtDescripcion.Text;
            incidencia.Estado = (int)Estado.Abierta;
            incidencia.Prioridad = PickerPrioridad.SelectedIndex;
            incidencia.FCreacion = DateTime.Now;
            //incidencia.Creada = usuarioLogeado    //TODO manejar el usuario logeadop en la aplicación

            _appShell.CrearIncidencia(incidencia);

            Navigation.PopModalAsync(true);
        }
        else
        {
            DisplayAlert(App.Current!.Resources.TryGetValue("falta_nombre", out object falta_nombre) ? (string)falta_nombre : "error",
                App.Current.Resources.TryGetValue("falta_nombre_desc", out object falta_nombre_desc) ? (string)falta_nombre_desc : "error",
                "OK");
        }
    }
}