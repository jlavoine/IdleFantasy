
namespace IdleFantasy {
    public static class BackendManager {

        public static IIdleFantasyBackend Backend;

        public static void Init( IIdleFantasyBackend i_backend ) {
            Backend = i_backend;
        }
    }
}
