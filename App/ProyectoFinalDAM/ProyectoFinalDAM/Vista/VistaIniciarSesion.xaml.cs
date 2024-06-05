using ProyectoFinalDAM.BaseDatos;

namespace ProyectoFinalDAM.Vista;

public partial class VistaIniciarSesion : ContentPage
{
    public VistaIniciarSesion()
    {
        InitializeComponent();
    }

    private void BtnInicarSesion_Clicked(object sender, EventArgs e)
    {
        _ = Mongo.IniciarSesion(TxtUsuario.Text, TxtContrasena.Text);
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