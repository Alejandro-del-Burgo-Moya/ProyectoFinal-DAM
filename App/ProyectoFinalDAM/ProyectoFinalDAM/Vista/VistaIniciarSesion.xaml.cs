using ProyectoFinalDAM.BaseDatos;

namespace ProyectoFinalDAM.Vista;

public partial class VistaIniciarSesion : ContentPage
{
    private readonly AppShell appShell;
    public VistaIniciarSesion(AppShell appShell)
    {
        InitializeComponent();
        this.appShell = appShell;
        TxtUsuario.Text = "adburgom01@gmail.com";
        TxtContrasena.Text = "";
    }

    private void BtnIniciarSesion_Clicked(object sender, EventArgs e)
    {
        _ = Mongo.IniciarSesion(TxtUsuario.Text, TxtContrasena.Text);
        appShell.SesionIniciada();
    }

    private void BtnMostrarOcultar_Pressed(object sender, EventArgs e)
    {
        TxtContrasena.IsPassword = false;
    }

    private void BtnMostrarOcultar_Released(object sender, EventArgs e)
    {
        TxtContrasena.IsPassword = true;
    }
}