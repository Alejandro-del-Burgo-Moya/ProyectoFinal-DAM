using ProyectoFinalDAM.BaseDatos;
using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Enums;
using System.Text.RegularExpressions;

namespace ProyectoFinalDAM.Vista;

public partial class VistaCrearUsuario : ContentPage
{
    private readonly Persona persona;

    [GeneratedRegex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")]
    private static partial Regex RegexEmail();

    [GeneratedRegex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{6,}$")]
    private static partial Regex RegexContrasena();

    public VistaCrearUsuario()
    {
        InitializeComponent();

        persona = new();

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
            if (!String.IsNullOrEmpty(txtApellido1Usuario.Text))
            {
                if (ValidarEmail())
                {
                    if (ValidarContrasena())
                    {
                        persona.Nombre = TxtNombreUsuario.Text;
                        persona.Email = TxtEmailUsuario.Text;
                        persona.Contrasena = TxtContrasenaUsuario.Text;
                        persona.Rol = PickerRolUsuario.SelectedIndex;

                        _ = Mongo.AgregarPersonaAsync(persona);

                        Navigation.PopModalAsync(true);
                    }
                    else
                    {
                        DisplayAlert(App.Current!.Resources.TryGetValue("falta_contrasena_usuario", out object falta_contrasena_usuario) ? (string)falta_contrasena_usuario : "error",
                        App.Current.Resources.TryGetValue("falta_contrasena_usuario_desc", out object falta_contrasena_usuario_desc) ? (string)falta_contrasena_usuario_desc : "error",
                        "OK");
                    }
                }
                else
                {
                    DisplayAlert(App.Current!.Resources.TryGetValue("falta_email_usuario", out object falta_email_usuario) ? (string)falta_email_usuario : "error",
                    App.Current.Resources.TryGetValue("falta_email_usuario_desc", out object falta_email_usuario_desc) ? (string)falta_email_usuario_desc : "error",
                    "OK");
                }
            }
            else
            {
                DisplayAlert(App.Current!.Resources.TryGetValue("falta_apellido1_usuario", out object falta_apellido1_usuario) ? (string)falta_apellido1_usuario : "error",
                App.Current.Resources.TryGetValue("falta_apellido1_usuario_desc", out object falta_apellido1_usuario_desc) ? (string)falta_apellido1_usuario_desc : "error",
                "OK");
            }
        }
        else
        {
            DisplayAlert(App.Current!.Resources.TryGetValue("falta_nombre_usuario", out object falta_nombre_usuario) ? (string)falta_nombre_usuario : "error",
                App.Current.Resources.TryGetValue("falta_nombre_usuario_desc", out object falta_nombre_usuario_desc) ? (string)falta_nombre_usuario_desc : "error",
                "OK");
        }
    }
}