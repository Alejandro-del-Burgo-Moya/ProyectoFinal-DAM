using ProyectoFinalDAM.BaseDatos;
using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Enums;

namespace ProyectoFinalDAM.Vista;

public partial class VistaIncidencias : ContentPage
{
    private readonly Mongo mongo;

    public VistaIncidencias(Mongo mongo)
    {
        InitializeComponent();

        this.mongo = mongo;
        InicializarPickers();
        RellenarListaIncidencias(mongo.LeerIncidencias());

        //Thread hiloRefrescarListaIncidencias = new(() => RefrescarListaIncidencias(mongo));
        //hiloRefrescarListaIncidencias.Start();
    }

    #region "Métodos privados"

    private void InicializarPickers()
    {
        List<string> filtroOrden =
        [
            "Más recientes primero",
            "Más antiguas priemro",
            "Más prioritarias primero",
            "Menos prioritarias primero",
            "Menos avanzadas primero",
            "Más avanzadas primero",
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
            ListaIncidencias.Add(VistaIncidencias.GenerarFrameIncidencia(incidencia));
        }
    }

    private static Frame GenerarFrameIncidencia(Incidencia incidencia)
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

        Label descripcionIncidencia = new() { Text = incidencia.Decripcion };
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

        Frame frame = new() { Content = grid };

        return frame;
    }

    #endregion

    #region "Eventos"

    private void PickerFiltroEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        RellenarListaIncidencias(mongo.LeerIncidenciasFiltroEstado((Estado)PickerFiltroEstado.SelectedIndex));
    }

    private void PickerFiltroPrioridad_SelectedIndexChanged(object sender, EventArgs e)
    {
        RellenarListaIncidencias(mongo.LeerIncidenciasFiltroPrioridad((Prioridad)PickerFiltroPrioridad.SelectedIndex));
    }

    private void PickerOrden_SelectedIndexChanged(object sender, EventArgs e)
    {
        RellenarListaIncidencias(mongo.LeerIncidenciasOrden(PickerOrden.SelectedIndex));
    }

    private void BtnBorrarFiltros_Clicked(object sender, EventArgs e)
    {
        InicializarPickerEstado();
        InicializarPickerPrioridad();
        PickerOrden.SelectedIndex = 0;
        RellenarListaIncidencias(mongo.LeerIncidencias());
    }

    private void BtnCrearIncidencia_Clicked(object sender, EventArgs e)
    {
        _ = Navigation.PushModalAsync(new VistaCrearIncidencia(mongo), true);
        RellenarListaIncidencias(mongo.LeerIncidencias());
    }

    #endregion
}