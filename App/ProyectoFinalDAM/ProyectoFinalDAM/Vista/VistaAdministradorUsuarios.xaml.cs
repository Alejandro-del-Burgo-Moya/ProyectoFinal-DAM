using MongoDB.Bson;
using ProyectoFinalDAM.BaseDatos;
using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Enums;

namespace ProyectoFinalDAM.Vista;

public partial class VistaAdministradorUsuarios : ContentPage
{
    public VistaAdministradorUsuarios()
    {
        InitializeComponent();
    }


    private async Task RellenarListaUsuarios()
    {
        var lista = await Mongo.LeerPersonasFiltroNombre(TxtBuscarGesUsu.Text);
        ListaAdministradorUsuarios.Children.Clear();
        foreach (var persona in lista)
        {
            ListaAdministradorUsuarios.Add(GenerarFramePersona(persona));
        }
    }


    public static Frame GenerarFramePersona(Persona persona)
    {
        Grid grid = new()
        {
            ColumnDefinitions =
                {
                    new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto)},
                    new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)},
                },

            RowDefinitions =
                {
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) },
                },

            ColumnSpacing = 10,
            RowSpacing = 10,
            ClassId = persona.Id.ToString(),
        };

        //Nombre de la persona
        Label nombre = new() { Text = App.Current!.Resources.TryGetValue("nombre_persona", out object nombre_persona) ? (string)nombre_persona : "Nombre" };
        grid.Add(nombre, 0, 0);

        Label nombrePersona = new() { Text = persona.NombreCompleto };
        grid.Add(nombrePersona, 1, 0);

        //Email de la persona
        Label email = new() { Text = App.Current!.Resources.TryGetValue("email_persona", out object email_persona) ? (string)email_persona : "Email" };
        grid.Add(email, 0, 1);

        Label emailPersona = new() { Text = persona.Email };
        grid.Add(emailPersona, 1, 1);

        //Rol de la persona
        Label rol = new() { Text = App.Current!.Resources.TryGetValue("rol_persona", out object rol_persona) ? (string)rol_persona : "Rol" };
        grid.Add(rol, 0, 2);

        Label rolPersona = new() { Text = Enum.GetName(typeof(Rol), persona.Rol) };
        grid.Add(rolPersona, 1, 2);

        //Menú contextual
        MenuFlyoutItem MFIPersonaBorrar = new()
        {
            Text = "Borrar",
        };
        MFIPersonaBorrar.Clicked += MFIPersonaBorrar_Clicked;

        MenuFlyoutItem MFIPersonaModifcar = new()
        {
            Text = "Modificar",
        };
        MFIPersonaModifcar.Clicked += MFIPersonaModifcar_Clicked;

        MenuFlyout mf = [MFIPersonaBorrar, MFIPersonaModifcar];

        FlyoutBase.SetContextFlyout(grid, mf);

        Frame frame = new() { Content = grid };

        return frame;
    }







    private static void MFIPersonaModifcar_Clicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private static void MFIPersonaBorrar_Clicked(object? sender, EventArgs e)
    {
        _ = Mongo.BorrarPersona(new ObjectId(((Grid)((Microsoft.Maui.Controls.Element)sender!).Parent.Parent).ClassId));
    }


    private void BuscarGesUsu_Clicked(object sender, EventArgs e)
    {
        _ = RellenarListaUsuarios();
    }


    private void BtnAgregarUsuario_Clicked(object sender, EventArgs e)
    {
        _ = Navigation.PushModalAsync(new VistaCrearUsuario(), true);

        _ = RellenarListaUsuarios();
    }


    private void ActualizarLista_Clicked(object sender, EventArgs e)
    {
        TxtBuscarGesUsu.Text = null;

        _ = RellenarListaUsuarios();
    }
}