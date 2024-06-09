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
        private User? user = null;

        private List<Incidencia> listaIncidencias = [];
        private List<Persona> listaPersonas = [];

        private readonly AppShell _appShell = appShell;

        public async Task<bool> IniciarSesion(string email, string contrasena)
        {
            _appShell.UsuarioLogueado.EstaLogueado = false;

            try
            {
                realm ??= RealmDatabaseService.GetRealm();
                await CerrarSesion();

                var lista = await LeerPersonas();
                if (lista.Where(p =>
                    string.Equals(p.Email, email, StringComparison.CurrentCultureIgnoreCase)
                    && string.Equals(p.Contrasena, contrasena, StringComparison.CurrentCultureIgnoreCase)) == null)
                {
                    return false;
                }

                if (App.RealmApp.CurrentUser == null)
                {
                    var user = await App.RealmApp.LogInAsync(Credentials.EmailPassword(email, contrasena));
                }
                else
                {
                    App.RealmApp.SwitchUser(await App.RealmApp.LogInAsync(Credentials.EmailPassword(email, contrasena)));
                }
                _appShell.UsuarioLogueado.Email = email;
                _appShell.UsuarioLogueado.Contrasena = contrasena;
                _appShell.UsuarioLogueado.EstaLogueado = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                ManejadorExcepciones.Manejar(ex);
#endif
            }

            return _appShell.UsuarioLogueado.EstaLogueado;
        }

        public async Task<bool> RegistrarUsuario(string email, string contrasena)
        {
            _appShell.UsuarioLogueado.EstaLogueado = false;
            _appShell.UsuarioLogueado.EstaResgistrado = false;
            try
            {
                ////realm ??= RealmDatabaseService.GetRealm();
                //await App.RealmApp.EmailPasswordAuth.RegisterUserAsync(email, contrasena);
                //_appShell.UsuarioLogueado.EstaResgistrado = true;

                ////realm ??= RealmDatabaseService.GetRealm();
                //if (App.RealmApp.CurrentUser == null)
                //{
                //    var user = await App.RealmApp.LogInAsync(Credentials.EmailPassword(email, contrasena));
                //}
                //else
                //{
                //    App.RealmApp.SwitchUser(await App.RealmApp.LogInAsync(Credentials.EmailPassword(email, contrasena)));
                //}

                await CerrarSesion();
                if (App.RealmApp.CurrentUser == null)
                {
                    await App.RealmApp.EmailPasswordAuth.RegisterUserAsync(email, contrasena);
                    _appShell.UsuarioLogueado.EstaResgistrado = true;
                    user = await App.RealmApp.LogInAsync(Credentials.EmailPassword(email, contrasena));
                }
                else
                {
                    user = App.RealmApp.CurrentUser;
                }

                _appShell.UsuarioLogueado.Email = email;
                _appShell.UsuarioLogueado.Contrasena = contrasena;
                _appShell.UsuarioLogueado.EstaLogueado = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                ManejadorExcepciones.Manejar(ex);
#endif
            }

            return _appShell.UsuarioLogueado.EstaResgistrado && _appShell.UsuarioLogueado.EstaLogueado;
        }

        public async Task CerrarSesion()
        {
            try
            {
                realm ??= RealmDatabaseService.GetRealm();
                //if (App.RealmApp.CurrentUser != null)
                if (user != null)
                {
                    await App.RealmApp.CurrentUser!.LogOutAsync();
                }
            }
            catch (Exception ex)
            {
                ManejadorExcepciones.Manejar(ex);
            }
        }



        #region "Incidencias"

        public async Task<List<Incidencia>> LeerIncidencias(int? estado = null, int? prioridad = null, int? orden = null, string? nombre = null)
        {
            realm!.Subscriptions.Update(() =>
            {
                var lista = realm.All<Incidencia>();

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
            Incidencia? incidencia = null;
            realm!.Subscriptions.Update(() =>
            {
                var resultado = realm.All<Incidencia>().Where(i => i.Id == id);
                realm.Subscriptions.Add(resultado);
                incidencia = resultado as Incidencia;
            });

            await realm.Subscriptions.WaitForSynchronizationAsync();

            return incidencia;
        }

        public async Task<bool> CrearIncidenciaAsync(Incidencia incidencia)
        {
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

            listaIncidencias = await LeerIncidencias();

            return listaIncidencias.Contains(incidencia);
        }

        private async Task BorrarIncidencia(ObjectId id)
        {
            var temp = LeerIncidencias().Result;
            Incidencia incidenciaBorrar = (Incidencia)temp.Select(i => i.Id == id);

            await realm!.WriteAsync(() => { realm.Remove(incidenciaBorrar); });

        }

        public async Task<bool> BorrarIncidencia(Incidencia incidencia)
        {
            await BorrarIncidencia(incidencia.Id);

            listaIncidencias = await LeerIncidencias();

            return !listaIncidencias.Contains(incidencia);
        }

        public async Task AsignarIncidencia(Incidencia incidencia, Persona persona)
        {
            await realm!.WriteAsync(() =>
            {
                incidencia.Asignada = persona;
                incidencia.FAsignacion = DateTimeOffset.Now;
                incidencia.Estado = (int)Estado.Asignada;
            });
        }

        public async Task ResolverIncidenciaAdmin(Incidencia incidencia)
        {
            await realm!.WriteAsync(() =>
            {
                incidencia.Resuelta = incidencia.Asignada;
                incidencia.FResolucion = DateTimeOffset.Now;
                incidencia.Estado = (int)Estado.Resuelta;
            });
        }

        #endregion



        #region "Personas"

        public async Task<List<Persona>> LeerPersonas(string? nombre = null, string? email = null)
        {
            realm!.Subscriptions.Update(() =>
            {
                var lista = realm.All<Persona>();

                if (!string.IsNullOrWhiteSpace(nombre)) { lista = lista.Where(p => p.Nombre!.Contains(nombre, StringComparison.CurrentCultureIgnoreCase)); }
                if (!string.IsNullOrWhiteSpace(email)) { lista = lista.Where(p => string.Equals(p.Email, email, StringComparison.CurrentCultureIgnoreCase)); }

                realm.Subscriptions.Add(lista);
                listaPersonas = [.. lista];
            });

            await realm.Subscriptions.WaitForSynchronizationAsync();

            return listaPersonas;
        }

        public async Task<Persona?> LeerPersona(ObjectId id)
        {
            Persona? persona = null;
            realm!.Subscriptions.Update(() =>
            {
                var resultado = realm.All<Persona>().Where(p => p.Id == id);
                realm.Subscriptions.Add(resultado);
                persona = resultado as Persona;
            });

            await realm.Subscriptions.WaitForSynchronizationAsync();

            return persona;
        }

        public async Task<bool> AgregarPersonaAsync(Persona persona)
        {
            realm ??= RealmDatabaseService.GetRealm();

            await realm.WriteAsync(() => { realm.Add(persona); });

            listaPersonas = await LeerPersonas();

            return listaPersonas.Contains(persona);
        }

        private async Task BorrarPersona(ObjectId id)
        {
            var temp = LeerPersonas().Result;
            Persona personaBorrar = (Persona)temp.Select(p => p.Id == id);

            await realm!.WriteAsync(() => realm.Remove(personaBorrar));
        }

        public async Task<bool> BorrarPersona(Persona persona)
        {
            await BorrarPersona(persona.Id);

            listaPersonas = await LeerPersonas();

            return !listaPersonas.Contains(persona);
        }

        #endregion
    }
}

