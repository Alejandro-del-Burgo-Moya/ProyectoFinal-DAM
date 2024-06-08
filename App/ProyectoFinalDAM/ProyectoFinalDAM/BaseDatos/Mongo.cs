using MongoDB.Bson;
using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Enums;
using ProyectoFinalDAM.Modelo.Excepciones;
using Realms;
using Realms.Sync;
using static ProyectoFinalDAM.Servicios.ServicioRealm;

namespace ProyectoFinalDAM.BaseDatos
{
    public class Mongo(AppShell appShell)
    {
        private Realm? realm = null;

        private List<Incidencia> listaIncidencias = [];
        private List<Persona> listaPersonas = [];

        private readonly AppShell _appShell = appShell;

        public async Task IniciarSesion(string email, string contrasena)
        {
            _appShell.UsuarioLogueado.NuevoUsuario = true;
            _appShell.UsuarioLogueado.Email = email;
            _appShell.UsuarioLogueado.Contrasena = contrasena;
            await Mongo.CerrarSesion();
            try
            {
                await App.RealmApp.EmailPasswordAuth.RegisterUserAsync(email, contrasena);
            }
            catch (Exception ex)
            {
                _appShell.UsuarioLogueado.NuevoUsuario = false;
#if DEBUG
                ManejadorExcepciones.Manejar(ex);
#endif
            }

            App.RealmApp.SwitchUser(await App.RealmApp.LogInAsync(Credentials.EmailPassword(email, contrasena)));

            _appShell.UsuarioLogueado.EstaLogueado = true;
        }

        public static async Task CerrarSesion()
        {
            if (App.RealmApp.CurrentUser != null)
            {
                await App.RealmApp.CurrentUser.LogOutAsync();
            }
        }



        #region "Incidencias"

        public async Task<List<Incidencia>> LeerIncidencias()
        {
            realm ??= RealmDatabaseService.GetRealm();
            realm.Subscriptions.Update(() =>
            {
                var lista = realm!.All<Incidencia>();
                realm.Subscriptions.Add(lista);
                listaIncidencias = [.. lista];
            });

            await realm.Subscriptions.WaitForSynchronizationAsync();

            return listaIncidencias;
        }

        public async Task<List<Incidencia>> BuscarIncidencias(int? estado = null, int? prioridad = null, int? orden = null, string? nombre = null)
        {
            realm ??= RealmDatabaseService.GetRealm();

            realm.Subscriptions.Update(() =>
            {
                var lista = realm!.All<Incidencia>();

                if (estado != null) { lista = lista.Where(i => i.Estado == estado); }

                if (prioridad != null) { lista = lista.Where(i => i.Prioridad == prioridad); }

                if (orden != null)
                {
                    lista = orden switch
                    {
                        0 => lista.OrderBy(i => i.FCreacion),
                        1 => lista.OrderByDescending(i => i.FCreacion),
                        2 => lista.OrderByDescending(i => i.Prioridad),
                        3 => lista.OrderBy(i => i.Prioridad),
                        4 => lista.OrderByDescending(i => i.Estado),
                        5 => lista.OrderBy(i => i.Estado),
                        _ => throw new OrdenIncidenciasException("Se ha introducido un orden no controlado : " + orden),
                    };
                }

                if (!string.IsNullOrWhiteSpace(nombre)) { lista = lista.Where(i => i.Nombre!.Contains(nombre, StringComparison.CurrentCultureIgnoreCase)); }

                realm.Subscriptions.Add(lista);
                listaIncidencias = [.. lista];
            });

            await realm.Subscriptions.WaitForSynchronizationAsync();

            return listaIncidencias;
        }

        public async Task<Incidencia?> LeerIncidencia(ObjectId id)
        {
            realm ??= RealmDatabaseService.GetRealm();
            Incidencia? incidencia = null;
            realm.Subscriptions.Update(() =>
            {
                var resultado = realm!.All<Incidencia>().Where(i => i.Id == id);
                realm.Subscriptions.Add(resultado);
                incidencia = resultado as Incidencia;
            });

            await realm.Subscriptions.WaitForSynchronizationAsync();

            return incidencia;
        }

        public async Task CrearIncidenciaAsync(Incidencia incidencia)
        {
            realm ??= RealmDatabaseService.GetRealm();
            try
            {
                await realm!.WriteAsync(() => { realm.Add(incidencia); });
            }
            catch (Exception ex)
            {
#if DEBUG
                ManejadorExcepciones.Manejar(ex);
#endif
            }
        }

        public async Task BorrarIncidencia(ObjectId id)
        {
            realm ??= RealmDatabaseService.GetRealm();

            var temp = LeerIncidencias().Result;
            Incidencia incidenciaBorrar = (Incidencia)temp.Select(i => i.Id == id);

            await realm!.WriteAsync(() => { realm.Remove(incidenciaBorrar); });

        }

        public async Task BorrarIncidencia(Incidencia incidencia)
        {
            realm ??= RealmDatabaseService.GetRealm();
            await realm!.WriteAsync(() => { realm.Remove(incidencia); });
        }

        public async Task AsignarIncidencia(Incidencia incidencia, Persona persona)
        {
            realm ??= RealmDatabaseService.GetRealm();

            await realm!.WriteAsync(() =>
            {
                incidencia.Asignada = persona;
                incidencia.FAsignacion = DateTimeOffset.Now;
                incidencia.Estado = (int)Estado.Asignada;
            });
        }

        public async Task ResolverIncidenciaAdmin(Incidencia incidencia)
        {
            realm ??= RealmDatabaseService.GetRealm();

            await realm!.WriteAsync(() =>
            {
                incidencia.Resuelta = incidencia.Asignada;
                incidencia.FResolucion = DateTimeOffset.Now;
                incidencia.Estado = (int)Estado.Resuelta;
            });
        }

        #endregion



        #region "Personas"

        public async Task<List<Persona>> BuscarPersonas(string? nombre = null)
        {
            realm ??= RealmDatabaseService.GetRealm();

            realm.Subscriptions.Update(() =>
            {
                var lista = realm!.All<Persona>();

                if (!string.IsNullOrWhiteSpace(nombre)) { lista = lista.Where(p => p.Nombre!.Contains(nombre, StringComparison.CurrentCultureIgnoreCase)); }

                realm.Subscriptions.Add(lista);
                listaPersonas = [.. lista];
            });

            await realm.Subscriptions.WaitForSynchronizationAsync();

            return listaPersonas;
        }

        public async Task<Persona?> LeerPersona(ObjectId id)
        {
            realm ??= RealmDatabaseService.GetRealm();
            Persona? persona = null;
            realm.Subscriptions.Update(() =>
            {
                var resultado = realm!.All<Persona>().Where(p => p.Id == id);
                realm.Subscriptions.Add(resultado);
                persona = resultado as Persona;
            });

            await realm.Subscriptions.WaitForSynchronizationAsync();

            return persona;
        }

        public async Task AgregarPersonaAsync(Persona persona)
        {
            realm ??= RealmDatabaseService.GetRealm();

            await realm!.WriteAsync(() => { realm.Add(persona); });
        }

        public async Task BorrarPersona(ObjectId id)
        {
            realm ??= RealmDatabaseService.GetRealm();

            var temp = BuscarPersonas().Result;
            Persona personaBorrar = (Persona)temp.Select(p => p.Id == id);

            await realm!.WriteAsync(() => realm.Remove(personaBorrar));
        }

        public async Task BorrarPersona(Persona persona)
        {
            realm ??= RealmDatabaseService.GetRealm();
            await realm!.WriteAsync(() => realm.Remove(persona));
        }

        #endregion
    }
}

