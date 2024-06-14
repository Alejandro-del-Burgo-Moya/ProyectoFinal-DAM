using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Vista.Modificar;

namespace ProyectoFinalDAM.Vista;

public partial class VistaAdministradorUsuarios : ContentPage
{
    private readonly AppShell _appShell;

    public VistaAdministradorUsuarios(AppShell appShell)
    {
        InitializeComponent();

        _appShell = appShell;

        InicializarPickerRol();
        RellenarListaUsuarios();
    }


    private void InicializarPickerRol()
    {
        List<string> filtroRol = Utiles.NombreRol();
        PickerRol.ItemsSource = null;
        PickerRol.ItemsSource = filtroRol;
    }


    private void RellenarListaUsuarios()
    {
        var lista = _appShell.LeerPersonas(TxtBuscarGesUsu.Text, PickerRol.SelectedIndex);
        ListaAdministradorUsuarios.ItemsSource = null;
        ListaAdministradorUsuarios.ItemsSource = lista;
    }

    private static void MostrarErrorNoSeleccionado()
    {
        Utiles.MostrarAdvertencia(Utiles.ExtraerValorDiccionario("error"), Utiles.ExtraerValorDiccionario("error_no_seleccionado"));
    }


    private void BuscarGesUsu_Clicked(object sender, EventArgs e)
    {
        RellenarListaUsuarios();
    }


    private void BtnAgregarUsuario_Clicked(object sender, EventArgs e)
    {
        _ = Navigation.PushModalAsync(new VistaCrearUsuario(_appShell), true);

        RellenarListaUsuarios();
    }


    private void BtnBorrarUsuario_Clicked(object sender, EventArgs e)
    {
        if (ListaAdministradorUsuarios.SelectedItem != null)
        {
            Persona persona = (Persona)ListaAdministradorUsuarios.SelectedItem;
            _appShell.BorrarPersona(persona);
            RellenarListaUsuarios();
        }
        else
        {
            VistaAdministradorUsuarios.MostrarErrorNoSeleccionado();
        }
    }


    private void BtnModificarUsuario_Clicked(object sender, EventArgs e)
    {
        if (ListaAdministradorUsuarios.SelectedItem != null)
        {
            Persona persona = (Persona)ListaAdministradorUsuarios.SelectedItem;
            _ = Navigation.PushModalAsync(new VistaModificarUsuario(_appShell, persona));
            RellenarListaUsuarios();
        }
        else
        {
            VistaAdministradorUsuarios.MostrarErrorNoSeleccionado();
        }
    }


    private void BtnBorrarFiltros_Clicked(object sender, EventArgs e)
    {
        InicializarPickerRol();
        TxtBuscarGesUsu.Text = null;
        RellenarListaUsuarios();
    }

    private void PickerRol_SelectedIndexChanged(object sender, EventArgs e)
    {
        RellenarListaUsuarios();
    }
}