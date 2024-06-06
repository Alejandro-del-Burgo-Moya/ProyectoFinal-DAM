using MongoDB.Bson;
using ProyectoFinalDAM.BaseDatos;
using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Enums;

namespace ProyectoFinalDAM.Vista;

public partial class VistaAdministradorIncidencias : ContentPage
{
    public VistaAdministradorIncidencias()
    {
        InitializeComponent();

        var lista = Mongo.LeerIncidencias();
        lista.Wait();
        RellenarListaIncidencias(lista.Result);
    }



    private void RellenarListaIncidencias(List<Incidencia> incidencias)
    {
        ListaAdministradorIncidencias.Children.Clear();
        foreach (var incidencia in incidencias)
        {
            ListaAdministradorIncidencias.Add(GenerarFrameIncidencia(incidencia));
        }
    }


    public static Frame GenerarFrameIncidencia(Incidencia incidencia)
    {
        Grid grid = new()
        {
            ColumnDefinitions =
                {
                    new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto)},
                    new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto)},
                    new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star)},
                },

            RowDefinitions =
                {
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto)},
                },

            ColumnSpacing = 10,
            RowSpacing = 10,
            ClassId = incidencia.Id.ToString(),
        };

        //Nombre de la incidencia
        Label nombre = new() { Text = App.Current!.Resources.TryGetValue("nombre_incidencia", out object nombre_incidencia) ? (string)nombre_incidencia : "Nombre" };
        grid.Add(nombre, 0, 0);

        Label nombreIncidencia = new() { Text = incidencia.Nombre };
        grid.Add(nombreIncidencia);
        grid.SetRow(nombreIncidencia, 0);
        grid.SetColumn(nombreIncidencia, 1);
        grid.SetColumnSpan(nombreIncidencia, 3);

        //Descripción de la incidencia
        Label descripcion = new() { Text = App.Current!.Resources.TryGetValue("descripcion_incidencia", out object descripcion_incidencia) ? (string)descripcion_incidencia : "Descripción" };
        grid.Add(descripcion, 0, 1);

        Label descripcionIncidencia = new() { Text = incidencia.Decripcion?.Split("\r")[0] };
        grid.Add(descripcionIncidencia);
        grid.SetRow(descripcionIncidencia, 1);
        grid.SetColumn(descripcionIncidencia, 1);
        grid.SetColumnSpan(descripcionIncidencia, 3);

        //Estado de la incidencia
        Label estado = new() { Text = App.Current!.Resources.TryGetValue("estado_incidencia", out object texto_estado) ? (string)texto_estado : "Estado" };
        grid.Add(estado, 0, 2);

        Label estadoIncidencia = new() { Text = Enum.GetName(typeof(Estado), incidencia.Estado) };
        grid.Add(estadoIncidencia, 1, 2);

        //Prioridad de la incidencia
        Label prioridad = new() { Text = App.Current!.Resources.TryGetValue("prioridad_incidencia", out object texto_prioridad) ? (string)texto_prioridad : "Prioridad" };
        grid.Add(prioridad, 2, 2);

        Label prioridadIncidencia = new() { Text = Enum.GetName(typeof(Prioridad), incidencia.Prioridad) };
        grid.Add(prioridadIncidencia, 3, 2);

        //Menú contextual
        MenuFlyoutItem MFIIncidenciaBorrar = new()
        {
            Text = "Borrar",
        };
        MFIIncidenciaBorrar.Clicked += MFIIncidenciaBorrar_Clicked;

        MenuFlyoutItem MFIIncidenciaModificar = new()
        {
            Text = "Modificar",
        };
        MFIIncidenciaModificar.Clicked += MFIIncidenciaModificar_Clicked;

        MenuFlyoutItem MFIIncidenciaAsignar = new()
        {
            Text = "Asignar",
        };
        MFIIncidenciaAsignar.Clicked += MFIIncidenciaAsignar_Clicked;

        MenuFlyoutItem MFIIncidenciaResolver = new()
        {
            Text = "Resolver",
        };
        MFIIncidenciaResolver.Clicked += MFIIncidenciaResolver_Clicked;

        MenuFlyout mf = [MFIIncidenciaBorrar, MFIIncidenciaModificar, MFIIncidenciaAsignar, MFIIncidenciaResolver];

        FlyoutBase.SetContextFlyout(grid, mf);

        return new Frame { Content = grid };
    }




    private void BuscarGesInc_Clicked(object sender, EventArgs e)
    {
        var lista = Mongo.LeerIncidenciasFiltroOrdenNombre(null, null, null, TxtBuscarGesInc.Text);
        lista.Wait();
        RellenarListaIncidencias(lista.Result);
    }


    private void ActualizarListaIncidenciasAdm_Clicked(object sender, EventArgs e)
    {
        TxtBuscarGesInc.Text = null;
        var lista = Mongo.LeerIncidencias();
        lista.Wait();
        RellenarListaIncidencias(lista.Result);
    }



    private void BtnCrearIncidenciaAdministrador_Clicked(object sender, EventArgs e)
    {
        _ = Navigation.PushModalAsync(new VistaCrearIncidencia(), true);
        var lista = Mongo.LeerIncidencias();
        lista.Wait();
        RellenarListaIncidencias(lista.Result);
    }


    private static void MFIIncidenciaBorrar_Clicked(object? sender, EventArgs e)
    {
        _ = Mongo.BorrarIncidencia(new ObjectId(((Grid)((Microsoft.Maui.Controls.Element)sender!).Parent.Parent).ClassId));
    }


    private static void MFIIncidenciaModificar_Clicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }


    private static void MFIIncidenciaAsignar_Clicked(object? sender, EventArgs e)
    {
        ObjectId idIncidencia = new(((Grid)((Microsoft.Maui.Controls.Element)sender!).Parent.Parent).ClassId);
        var lista = Mongo.LeerIncidencias();
        lista.Wait();
        Incidencia incidencia = (Incidencia)lista.Result.Where(i => i.Id == idIncidencia).First();
        if (incidencia.Estado != (int)Estado.Resuelta)
        {
            var tecnicos = Mongo.LeerPersonas();
            tecnicos.Wait();
            string nombre = App.Current!.MainPage!.DisplayActionSheet(
                App.Current!.Resources.TryGetValue("asignar_tecnico", out object asignar_tecnico) ? (string)asignar_tecnico : "Elige un técnico",
                null,
                null,
                tecnicos.Result.Where(p => p.Rol == (int)Rol.Tecnico).Select(p => p.NombreCompleto).ToArray()
            ).Result;

            if (nombre != null)
            {
                Persona persona = (Persona)tecnicos.Result.Where(p => p.NombreCompleto == nombre).First();
                _ = Mongo.AsignarIncidencia(incidencia, persona);
            }
        }
        else
        {
            App.Current!.MainPage!.DisplayAlert(
                App.Current!.Resources.TryGetValue("error_asignar_incidencia_resuelta", out object error_asignar_incidencia_resuelta) ? (string)error_asignar_incidencia_resuelta : "error",
                App.Current.Resources.TryGetValue("error_asignar_incidencia_resuelta_desc", out object error_asignar_incidencia_resuelta_desc) ? (string)error_asignar_incidencia_resuelta_desc : "error",
                "OK"
            );
        }
    }


    private static void MFIIncidenciaResolver_Clicked(object? sender, EventArgs e)
    {
        ObjectId idIncidencia = new(((Grid)((Microsoft.Maui.Controls.Element)sender!).Parent.Parent).ClassId);
        var lista = Mongo.LeerIncidencias();
        lista.Wait();
        Incidencia incidencia = (Incidencia)lista.Result.Where(i => i.Id == idIncidencia).First();
        _ = Mongo.ResolverIncidenciaAdmin(incidencia);
    }
}