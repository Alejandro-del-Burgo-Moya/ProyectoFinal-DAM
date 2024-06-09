using ProyectoFinalDAM.BaseDatos;
using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Excepciones;

namespace ProyectoFinalDAM.Vista;

public partial class VistaIniciarSesion : ContentPage
{
    private readonly AppShell _appShell;
    public VistaIniciarSesion(AppShell appShell)
    {
        InitializeComponent();
        this._appShell = appShell;
        TxtUsuario.Text = "adburgom01@gmail.com";
        TxtContrasena.Text = "";   //TODO borrar antes de subir
    }

    private void BtnIniciarSesion_Clicked(object sender, EventArgs e)
    {
        if (_appShell.IniciarSesion(TxtUsuario.Text, TxtContrasena.Text))
        {
            _appShell.CargarMenuUsuario();
        }
        else
        {
            Application.Current!.MainPage!.DisplayAlert(
                "Error de inicio de sesi�n",
                "El usuario no existe",
                "OK");
        }
    }

    private void BtnMostrarOcultar_Pressed(object sender, EventArgs e)
    {
        TxtContrasena.IsPassword = false;
    }

    private void BtnMostrarOcultar_Released(object sender, EventArgs e)
    {
        TxtContrasena.IsPassword = true;
    }

    private bool CrearNuevaPersona()
    {
        VistaCrearUsuario vista = new(_appShell, TxtUsuario.Text, TxtContrasena.Text, true);
        _ = Navigation.PushModalAsync(vista, true);
        return vista.UsuarioCreadoCorrectamente;
    }

    private void BtnRegistrarUsuario_Clicked(object sender, EventArgs e)
    {
        if (CrearNuevaPersona())
        {
            if (_appShell.RegistrarUsuario(TxtUsuario.Text, TxtContrasena.Text))
            {
                _appShell.IniciarSesion(TxtUsuario.Text, TxtContrasena.Text);
            }
        }
    }
}