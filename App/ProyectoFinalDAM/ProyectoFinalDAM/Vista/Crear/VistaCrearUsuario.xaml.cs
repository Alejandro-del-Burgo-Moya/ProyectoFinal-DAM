using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Enums;
using System.Text.RegularExpressions;

namespace ProyectoFinalDAM.Vista;

public partial class VistaCrearUsuario : ContentPage
{
    private readonly AppShell _appShell;

    [GeneratedRegex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")]
    private static partial Regex RegexEmail();

    [GeneratedRegex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{6,}$")]
    private static partial Regex RegexContrasena();

    public Persona? personaNueva;

    public VistaCrearUsuario(AppShell appShell, string? email = null, string? contrasena = null)
    {
        InitializeComponent();

        _appShell = appShell;

        if (!string.IsNullOrWhiteSpace(email)) { TxtEmailUsuario.Text = email; }
        if (!string.IsNullOrWhiteSpace(contrasena)) { TxtContrasenaUsuario.Text = contrasena; }

        List<string> Roles = [.. Enum.GetNames<Rol>()];
        Roles.Remove(Rol.Administrador.ToString());
        PickerRolUsuario.ItemsSource = null;
        PickerRolUsuario.ItemsSource = Roles;
        PickerRolUsuario.SelectedIndex = 0;
    }

    /// <summary>
    /// Comprueba que se haya introducido un correo electrónico válido
    /// </summary>
    /// <returns>True si el correo introducido es válido, False si no.</returns>
    private bool ValidarEmail()
    {
        if (String.IsNullOrEmpty(TxtEmailUsuario.Text)) return false;
        return RegexEmail().Match(TxtEmailUsuario.Text).Success;
    }

    private bool ValidarContrasena()
    {
        if (String.IsNullOrEmpty(TxtContrasenaUsuario.Text)) return false;
        return RegexContrasena().Match(TxtContrasenaUsuario.Text).Success;
    }

    private void BtnBorrarCamposUsuario_Clicked(object sender, EventArgs e)
    {
        TxtNombreUsuario.Text = null;
        TxtEmailUsuario.Text = null;
        PickerRolUsuario.SelectedIndex = 0;
    }

    private void BtnCancelar_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync(true);
    }

    private void BtnCrearUsuario_Clicked(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(TxtNombreUsuario.Text))
        {
            if (!String.IsNullOrEmpty(TxtApellido1Usuario.Text))
            {
                if (ValidarEmail())
                {
                    if (ValidarContrasena())
                    {
                        personaNueva = new()
                        {
                            Nombre = TxtNombreUsuario.Text,
                            Apellido1 = TxtApellido1Usuario.Text,
                            Apellido2 = TxtApellido2Usuario.Text,
                            Email = TxtEmailUsuario.Text,
                            Contrasena = TxtContrasenaUsuario.Text,
                            Rol = PickerRolUsuario.SelectedIndex,
                        };

                        _appShell.RegistrarUsuario(personaNueva);

                        Navigation.PopModalAsync(true);
                    }
                    else
                    {
                        Utiles.MostrarAdvertencia(Utiles.ExtraerValorDiccionario("falta_contrasena_usuario"), Utiles.ExtraerValorDiccionario("falta_contrasena_usuario_desc"));
                    }
                }
                else
                {
                    Utiles.MostrarAdvertencia(Utiles.ExtraerValorDiccionario("falta_email_usuario"), Utiles.ExtraerValorDiccionario("falta_email_usuario_desc"));
                }
            }
            else
            {
                Utiles.MostrarAdvertencia(Utiles.ExtraerValorDiccionario("falta_apellido1_usuario"), Utiles.ExtraerValorDiccionario("falta_apellido1_usuario_desc"));
            }
        }
        else
        {
            Utiles.MostrarAdvertencia(Utiles.ExtraerValorDiccionario("falta_nombre_usuario"), Utiles.ExtraerValorDiccionario("falta_nombre_usuario_desc"));
        }
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