using ProyectoFinalDAM.Modelo;

namespace ProyectoFinalDAM.Vista;

public partial class VistaIniciarSesion : ContentPage
{
    private readonly AppShell _appShell;
    public VistaIniciarSesion(AppShell appShell)
    {
        InitializeComponent();
        this._appShell = appShell;
        TxtUsuario.Text = "adburgom01@gmail.com";
        TxtContrasena.Text = "B8nl7320c";   //TODO borrar antes de subir
    }

    private void BtnIniciarSesion_Clicked(object sender, EventArgs e)
    {
        if (_appShell.IniciarSesion(TxtUsuario.Text, TxtContrasena.Text))
        {
            _appShell.CargarMenuUsuario();
        }
        else
        {
            Utiles.MostrarAdvertencia(Utiles.ExtraerValorDiccionario("error_inicio_sesion"), Utiles.ExtraerValorDiccionario("error_usuario_no_existe"));
        }
    }

    private void BtnMostrarOcultar_Pressed(object sender, EventArgs e)
    {
        BtnMostrarOcultar.ImageSource = "mostrar.png";
        TxtContrasena.IsPassword = false;
    }

    private void BtnMostrarOcultar_Released(object sender, EventArgs e)
    {
        BtnMostrarOcultar.ImageSource = "ocultar.png";
        TxtContrasena.IsPassword = true;
    }

    private void BtnRegistrarUsuario_Clicked(object sender, EventArgs e)
    {
        VistaCrearUsuario vista = new(_appShell, TxtUsuario.Text, TxtContrasena.Text);
        _ = Navigation.PushModalAsync(vista, true);
    }
}