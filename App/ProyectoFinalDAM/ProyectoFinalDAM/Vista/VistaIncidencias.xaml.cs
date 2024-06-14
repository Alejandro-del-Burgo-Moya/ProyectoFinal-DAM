using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Vista.Modificar;

namespace ProyectoFinalDAM.Vista;

public partial class VistaIncidencias : ContentPage
{
    private readonly AppShell _appShell;
    private readonly bool _misIncidenciasAsignadas = false;
    private readonly bool _incidenciasCreadasPorMi = false;
    private int? estado = null;
    private int? prioridad = null;
    private int? orden = null;

    public VistaIncidencias(AppShell appShell, bool? misIncidenciasAsignadas = false, bool? incidenciasCreadasPorMi = false)
    {
        InitializeComponent();



        _appShell = appShell;
        if (misIncidenciasAsignadas != null) { _misIncidenciasAsignadas = misIncidenciasAsignadas.Value; }
        if (incidenciasCreadasPorMi != null) { _incidenciasCreadasPorMi = incidenciasCreadasPorMi.Value; }

        InicializarPickers();

        if ((_appShell.UsuarioLogueado!.Rol == 1 || _appShell.UsuarioLogueado.Rol == 2) && !_misIncidenciasAsignadas) { BtnAsignarmeIncidencia.IsVisible = true; }
        if ((_appShell.UsuarioLogueado.Rol == 1 || _appShell.UsuarioLogueado.Rol == 2) && _misIncidenciasAsignadas) { BtnResolverIncidencia.IsVisible = true; }
        if (!_misIncidenciasAsignadas && !_incidenciasCreadasPorMi) { BtnCrearIncidencia.IsVisible = true; }
        if (_misIncidenciasAsignadas) { this.Title = Utiles.ExtraerValorDiccionario("mis_incidencias"); }
        if (_incidenciasCreadasPorMi) { this.Title = Utiles.ExtraerValorDiccionario("incidencias_creadas_por_mi"); }
    }


    private void InicializarPickers()
    {
        List<string> filtroOrden =
        [
            Utiles.ExtraerValorDiccionario("picker_orden_incidencias1"),
            Utiles.ExtraerValorDiccionario("picker_orden_incidencias2"),
            Utiles.ExtraerValorDiccionario("picker_orden_incidencias3"),
            Utiles.ExtraerValorDiccionario("picker_orden_incidencias4"),
            Utiles.ExtraerValorDiccionario("picker_orden_incidencias5"),
            Utiles.ExtraerValorDiccionario("picker_orden_incidencias6"),
        ];
        PickerOrden.ItemsSource = filtroOrden;
        PickerOrden.SelectedIndex = 0;

        InicializarPickerEstado();
        InicializarPickerPrioridad();
    }


    private void InicializarPickerEstado()
    {
        List<string> filtroEstado = Utiles.NombresEstado();
        PickerFiltroEstado.ItemsSource = null;
        PickerFiltroEstado.ItemsSource = filtroEstado;
    }


    private void InicializarPickerPrioridad()
    {
        List<string> filtroPrioridad = Utiles.NombresPrioridad();
        PickerFiltroPrioridad.ItemsSource = null;
        PickerFiltroPrioridad.ItemsSource = filtroPrioridad;
    }


    private void RellenarListaIncidencias()
    {
        var lista = _appShell.LeerIncidencias(estado, prioridad, orden, TxtBuscar.Text, _misIncidenciasAsignadas, _incidenciasCreadasPorMi);
        ListaIncidencias.ItemsSource = null;
        ListaIncidencias.ItemsSource = lista;
    }

    private static void MostrarErrorNoSeleccionado()
    {
        Utiles.MostrarAdvertencia(Utiles.ExtraerValorDiccionario("error"), Utiles.ExtraerValorDiccionario("error_no_seleccionado"));
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
            if (incidencia.Estado != 3)
            {
                _ = Navigation.PushModalAsync(new VistaModificarIncidencia(_appShell, incidencia));
            }
            else
            {
                Utiles.MostrarAdvertencia(Utiles.ExtraerValorDiccionario("error"), Utiles.ExtraerValorDiccionario("error_modificar_incidencia_resuelta"));
            }
        }
        else
        {
            MostrarErrorNoSeleccionado();
        }
    }

    private void BtnAsignarmeIncidencia_Clicked(object sender, EventArgs e)
    {
        if (ListaIncidencias.SelectedItem != null)
        {
            Incidencia incidencia = (Incidencia)ListaIncidencias.SelectedItem;
            if (incidencia.Estado != 3)
            {
                _appShell.AsignarmeIncidencia(incidencia);
            }
            else
            {
                Utiles.MostrarAdvertencia(Utiles.ExtraerValorDiccionario("error"), Utiles.ExtraerValorDiccionario("error_asignar_incidencia_resuelta"));
            }
        }
        else
        {
            MostrarErrorNoSeleccionado();
        }
    }

    private void BtnResolverIncidencia_Clicked(object sender, EventArgs e)
    {
        if (ListaIncidencias.SelectedItem != null)
        {
            Incidencia incidencia = (Incidencia)ListaIncidencias.SelectedItem;
            if (incidencia.Estado != 3)
            {
                _appShell.ResolverIncidencia(incidencia);
            }
            else
            {
                Utiles.MostrarAdvertencia(Utiles.ExtraerValorDiccionario("error"), Utiles.ExtraerValorDiccionario("error_resolver_incidencia_resuelta"));
            }
        }
        else
        {
            MostrarErrorNoSeleccionado();
        }
    }
}