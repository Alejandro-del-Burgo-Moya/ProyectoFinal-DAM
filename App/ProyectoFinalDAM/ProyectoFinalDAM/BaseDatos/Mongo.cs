using Microsoft.Maui.Controls;
using MongoDB.Bson;
using MongoDB.Driver;
using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Enums;
using ProyectoFinalDAM.Modelo.Excepciones;
using Realms;
using Realms.Sync;
using static ProyectoFinalDAM.Servicios.ServicioRealm;
using MongoClient = MongoDB.Driver.MongoClient;

namespace ProyectoFinalDAM.BaseDatos
{
    public static class Mongo
    {
        //private static readonly string URIConexion = "mongodb+srv://adburgom01:b8nl7320c@proyectofinaldam.q49ehh3.mongodb.net/?retryWrites=true&w=majority&appName=ProyectoFinalDAM";

        private static User? user;
        private static Realm? realm;

        private static List<Incidencia> listaIncidencias = [];
        private static List<Persona> listaPersonas = [];

        public static async Task IniciarSesion(string email, string contrasena)
        {
            await CerrarSesion();
            if (App.RealmApp.CurrentUser == null)
            {
                try
                {
                    await App.RealmApp.EmailPasswordAuth.RegisterUserAsync(email, contrasena);
                }
                catch (Exception) { }

                user = await App.RealmApp.LogInAsync(Credentials.EmailPassword(email, contrasena));
            }
            else
            {
                user = App.RealmApp.CurrentUser;
            }
        }

        public static async Task CerrarSesion()
        {
            if (user != null)
            {
                await user.LogOutAsync();
            }
        }



        #region "Incidencias"

        public static async Task<List<Incidencia>> LeerIncidencias()
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

        public static async Task<List<Incidencia>> LeerIncidenciasFiltroOrdenNombre(int? estado = null, int? prioridad = null, int? orden = null, string? nombre = null)
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

        public static async Task CrearIncidenciaAsync(Incidencia incidencia)
        {
            realm ??= RealmDatabaseService.GetRealm();
            await realm!.WriteAsync(() => { realm.Add(incidencia); });
        }

        public static async Task BorrarIncidencia(ObjectId id)
        {
            realm ??= RealmDatabaseService.GetRealm();

            var temp = LeerIncidencias().Result;
            Incidencia incidenciaBorrar = (Incidencia)temp.Select(i => i.Id == id);

            await realm!.WriteAsync(() => { realm.Remove(incidenciaBorrar); });

        }

        public static async Task BorrarIncidencia(Incidencia incidencia)
        {
            realm ??= RealmDatabaseService.GetRealm();
            await realm!.WriteAsync(() => { realm.Remove(incidencia); });
        }

        public static async Task AsignarIncidencia(Incidencia incidencia, Persona persona)
        {
            realm ??= RealmDatabaseService.GetRealm();

            await realm!.WriteAsync(() =>
            {
                incidencia.Asignada = persona;
                incidencia.FAsignacion = DateTimeOffset.Now;
                incidencia.Estado = (int)Estado.Asignada;
            });
        }

        public static async Task ResolverIncidenciaAdmin(Incidencia incidencia)
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

        public static async Task<List<Persona>> LeerPersonasFiltroNombre(string? nombre = null)
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

        public static async Task AgregarPersonaAsync(Persona persona)
        {
            realm ??= RealmDatabaseService.GetRealm();
            await realm!.WriteAsync(() => { realm.Add(persona); });
        }

        public static async Task BorrarPersona(ObjectId id)
        {
            realm ??= RealmDatabaseService.GetRealm();

            var temp = LeerPersonasFiltroNombre().Result;
            Persona personaBorrar = (Persona)temp.Select(p => p.Id == id);

            await realm!.WriteAsync(() => realm.Remove(personaBorrar));
        }

        public static async Task BorrarPersona(Persona persona)
        {
            realm ??= RealmDatabaseService.GetRealm();
            await realm!.WriteAsync(() => realm.Remove(persona));
        }

        #endregion
    }
}

