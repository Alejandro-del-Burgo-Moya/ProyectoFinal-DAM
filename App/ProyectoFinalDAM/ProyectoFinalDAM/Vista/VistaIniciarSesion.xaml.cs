using ProyectoFinalDAM.BaseDatos;
using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Excepciones;

namespace ProyectoFinalDAM.Vista;

public partial class VistaIniciarSesion : ContentPage
{
    private readonly AppShell appShell;
    public VistaIniciarSesion(AppShell appShell)
    {
        InitializeComponent();
        this.appShell = appShell;
        TxtUsuario.Text = "adburgom01@gmail.com";
        TxtContrasena.Text = "";   //TODO borrar antes de subir
    }

    private void BtnIniciarSesion_Clicked(object sender, EventArgs e)
    {
        appShell.IniciarSesion(TxtUsuario.Text, TxtContrasena.Text);

        if (appShell.UsuarioLogueado.NuevoUsuario)
        {
            try
            {
                if (CrearNuevaPersona())
                {
                    appShell.UsuarioLogueado.NuevoUsuario = false;
                    appShell.CargarMenuUsuario();
                }
                else
                {
                    throw new CrearUsuarioException();
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                ManejadorExcepciones.Manejar(ex);
#endif
            }
        }
        else
        {
            appShell.CargarMenuUsuario();
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
        VistaCrearUsuario vista = new(appShell, appShell.UsuarioLogueado.Email, appShell.UsuarioLogueado.Contrasena, true);
        var task = Navigation.PushModalAsync(vista, true);
        //task.ConfigureAwait(ConfigureAwaitOptions.ForceYielding);
        //task.Wait();
        return vista.UsuarioCreadoCorrectamente;
    }
}