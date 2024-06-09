using Realms.Sync;
using Realms;
using ProyectoFinalDAM.Modelo;
using ProyectoFinalDAM.Modelo.Excepciones;

namespace ProyectoFinalDAM.Servicios
{
    public static class ServicioRealm
    {
        public static class RealmDatabaseService
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            /// <exception cref="UsuarioActualRealmException"></exception>
            public static Realm GetRealm()
            {
                if (App.RealmApp.CurrentUser != null)
                {
                    FlexibleSyncConfiguration config = new(App.RealmApp.CurrentUser)
                    {
                        PopulateInitialSubscriptions = (realm) =>
                        {
                            var incidencias = realm.All<Incidencia>();
                            var personas = realm.All<Persona>();
                            realm.Subscriptions.Add(incidencias);
                            realm.Subscriptions.Add(personas);
                        }
                    };
                    return Realm.GetInstance(config);
                }
                else { throw new UsuarioActualRealmException(); }
            }
        }
    }
}
