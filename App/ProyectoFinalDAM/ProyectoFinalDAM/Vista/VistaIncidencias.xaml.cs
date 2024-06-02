using ProyectoFinalDAM.BaseDatos;
using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Enums;

namespace ProyectoFinalDAM.Vista;

public partial class VistaIncidencias : ContentPage
{
    public VistaIncidencias()
    {
        InitializeComponent();

        InicializarPickers();
        RellenarListaIncidencias(Mongo.LeerIncidencias());
    }





    private void InicializarPickers()
    {
        List<string> filtroOrden =
        [
            "M�s recientes primero",
            "M�s antiguas primero",
            "M�s prioritarias primero",
            "Menos prioritarias primero",
            "M�s avanzadas primero",
            "Menos avanzadas primero",
        ];
        PickerOrden.ItemsSource = filtroOrden;
        PickerOrden.SelectedIndex = 0;

        InicializarPickerEstado();
        InicializarPickerPrioridad();
    }


    private void InicializarPickerEstado()
    {
        List<string> filtroEstado = [.. Enum.GetNames<Estado>()];
        PickerFiltroEstado.ItemsSource = null;
        PickerFiltroEstado.ItemsSource = filtroEstado;
    }


    private void InicializarPickerPrioridad()
    {
        List<string> filtroPrioridad = [.. Enum.GetNames<Prioridad>()];
        PickerFiltroPrioridad.ItemsSource = null;
        PickerFiltroPrioridad.ItemsSource = filtroPrioridad;
    }


    private void RellenarListaIncidencias(List<Incidencia> incidencias)
    {
        ListaIncidencias.Children.Clear();
        foreach (var incidencia in incidencias)
        {
            ListaIncidencias.Add(GenerarFrameIncidencia(incidencia));
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

        //Descripci�n de la incidencia
        Label descripcion = new() { Text = App.Current!.Resources.TryGetValue("descripcion_incidencia", out object descripcion_incidencia) ? (string)descripcion_incidencia : "Descripci�n" };
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

        return new Frame { Content = grid };
    }








    private void PickerFiltroEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        RellenarListaIncidencias(Mongo.LeerIncidenciasFiltroEstado((Estado)PickerFiltroEstado.SelectedIndex));
    }


    private void PickerFiltroPrioridad_SelectedIndexChanged(object sender, EventArgs e)
    {
        RellenarListaIncidencias(Mongo.LeerIncidenciasFiltroPrioridad((Prioridad)PickerFiltroPrioridad.SelectedIndex));
    }


    private void PickerOrden_SelectedIndexChanged(object sender, EventArgs e)
    {
        RellenarListaIncidencias(Mongo.LeerIncidenciasOrden(PickerOrden.SelectedIndex));
    }


    private void BtnBorrarFiltros_Clicked(object sender, EventArgs e)
    {
        InicializarPickerEstado();
        InicializarPickerPrioridad();
        PickerOrden.SelectedIndex = 0;
        TxtBuscar.Text = null;
        RellenarListaIncidencias(Mongo.LeerIncidencias());
    }


    private void BtnCrearIncidencia_Clicked(object sender, EventArgs e)
    {
        _ = Navigation.PushModalAsync(new VistaCrearIncidencia(), true);
        RellenarListaIncidencias(Mongo.LeerIncidencias());
    }


    private void Buscar_Clicked(object sender, EventArgs e)
    {
        RellenarListaIncidencias(Mongo.BuscarIncidencia(TxtBuscar.Text));
    }


    private void ActualizarListaIncidencias_Clicked(object sender, EventArgs e)
    {
        InicializarPickerEstado();
        InicializarPickerPrioridad();
        PickerOrden.SelectedIndex = 0;
        TxtBuscar.Text = null;
        RellenarListaIncidencias(Mongo.LeerIncidencias());
    }
}