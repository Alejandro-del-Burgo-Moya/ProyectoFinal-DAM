using ProyectoFinalDAM.Modelo;

namespace ProyectoFinalDAM.Vista.Modificar;

public partial class VistaModificarUsuario : ContentPage
{
    private readonly AppShell _appShell;
    private readonly Persona _persona;
    public VistaModificarUsuario(AppShell appShell, Persona persona)
    {
        InitializeComponent();

        _appShell = appShell;
        _persona = persona;

        InicializarPickerRol();

        TxtNombreUsuario.Text = _persona.Nombre;
        TxtApellido1Usuario.Text = _persona.Apellido1;
        TxtApellido2Usuario.Text = _persona.Apellido2;
        TxtEmailUsuario.Text = _persona.Email;
        TxtContrasenaUsuario.Text = _persona.Contrasena;
        PickerRol.SelectedIndex = _persona.Rol;
    }


    private void InicializarPickerRol()
    {
        List<string> filtroRol = Utiles.NombreRol();
        PickerRol.ItemsSource = null;
        PickerRol.ItemsSource = filtroRol;
    }

    private void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync(true);
    }

    private void BtnGuardarCambiosUsuario_Clicked(object sender, EventArgs e)
    {
        _persona.Email = TxtEmailUsuario.Text;
        _persona.Contrasena = TxtContrasenaUsuario.Text;
        _persona.Rol = PickerRol.SelectedIndex;

        _appShell.ModificarPersona(_persona);

        Navigation.PopModalAsync(true);
    }

    private void BtnMostrarOcultar_Pressed(object sender, EventArgs e)
    {
        TxtContrasenaUsuario.IsPassword = false;
    }

    private void BtnMostrarOcultar_Released(object sender, EventArgs e)
    {
        TxtContrasenaUsuario.IsPassword = true;
    }
}