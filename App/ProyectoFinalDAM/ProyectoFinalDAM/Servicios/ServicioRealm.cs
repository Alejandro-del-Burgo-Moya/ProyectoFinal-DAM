using Realms.Sync;
using Realms;

namespace ProyectoFinalDAM.Servicios
{
    public static class ServicioRealm
    {
        public static class RealmDatabaseService
        {
            public static Realm GetRealm()
            {
                //PartitionSyncConfiguration config = new($"{App.RealmApp.CurrentUser.Id}", App.RealmApp.CurrentUser);
                FlexibleSyncConfiguration config = new(App.RealmApp.CurrentUser);
                return Realm.GetInstance(config);
            }
        }
    }
}
