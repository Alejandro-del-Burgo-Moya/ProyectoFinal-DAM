using Realms.Sync;
using Realms;
using ProyectoFinalDAM.Modelo;

namespace ProyectoFinalDAM.Servicios
{
    public static class ServicioRealm
    {
        public static class RealmDatabaseService
        {
            public static Realm GetRealm()
            {
                //PartitionSyncConfiguration config = new($"{App.RealmApp.CurrentUser.Id}", App.RealmApp.CurrentUser);
                FlexibleSyncConfiguration config = new(App.RealmApp.CurrentUser!)
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
        }
    }
}
