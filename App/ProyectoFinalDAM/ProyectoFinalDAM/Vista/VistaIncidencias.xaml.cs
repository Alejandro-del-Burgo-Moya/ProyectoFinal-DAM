using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Enums;
using ProyectoFinalDAM.Vista.Modificar;

namespace ProyectoFinalDAM.Vista;

public partial class VistaIncidencias : ContentPage
{
    private readonly AppShell _appShell;
    private int? estado = null;
    private int? prioridad = null;
    private int? orden = null;

    public VistaIncidencias(AppShell appShell)
    {
        InitializeComponent();

        _appShell = appShell;

        InicializarPickers();
    }





    private void InicializarPickers()
    {
        List<string> filtroOrden =
        [
            "Más recientes primero",
            "Más antiguas primero",
            "Más prioritarias primero",
            "Menos prioritarias primero",
            "Más avanzadas primero",
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


    private void RellenarListaIncidencias()
    {
        var lista = _appShell.LeerIncidencias(estado, prioridad, orden, TxtBuscar.Text);
        //ListaIncidencias.Children.Clear();
        ListaIncidencias.ItemsSource = null;
        ListaIncidencias.ItemsSource = lista;
        //foreach (var incidencia in lista)
        //{
        //    ListaIncidencias.Add(GenerarFrameIncidencia(incidencia));
        //}
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

        Label descripcionIncidencia = new() { Text = incidencia.Descripcion?.Split("\r")[0] };
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
        estado = PickerFiltroEstado.SelectedIndex;
        RellenarListaIncidencias();
    }


    private void PickerFiltroPrioridad_SelectedIndexChanged(object sender, EventArgs e)
    {
        prioridad = PickerFiltroPrioridad.SelectedIndex;
        RellenarListaIncidencias();
    }


    private void PickerOrden_SelectedIndexChanged(object sender, EventArgs e)
    {
        orden = PickerOrden.SelectedIndex;
        RellenarListaIncidencias();
    }


    private void BtnBorrarFiltros_Clicked(object sender, EventArgs e)
    {
        InicializarPickerEstado();
        InicializarPickerPrioridad();
        PickerOrden.SelectedIndex = 0;
        TxtBuscar.Text = null;
        RellenarListaIncidencias();
    }


    private void BtnCrearIncidencia_Clicked(object sender, EventArgs e)
    {
        _ = Navigation.PushModalAsync(new VistaCrearIncidencia(_appShell), true);
        RellenarListaIncidencias();
    }


    private void Buscar_Clicked(object sender, EventArgs e)
    {
        RellenarListaIncidencias();
    }


    private void ActualizarListaIncidencias_Clicked(object sender, EventArgs e)
    {
        InicializarPickerEstado();
        InicializarPickerPrioridad();
        PickerOrden.SelectedIndex = 0;
        TxtBuscar.Text = null;

        RellenarListaIncidencias();
    }

    private void BtnModificarIncidencia_Clicked(object sender, EventArgs e)
    {
        if (ListaIncidencias.SelectedItem != null)
        {
            Incidencia incidencia = (Incidencia)ListaIncidencias.SelectedItem;
            _ = Navigation.PushModalAsync(new VistaModificarIncidencia(_appShell, incidencia));
        }
        else
        {
            Utiles.MostrarAdvertencia("Error", "Debes seleccionar una incidencia de la lista");
        }
    }

    private void ListaIncidencias_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        Utiles.MostrarAdvertencia("prueba", "tap");
    }
}