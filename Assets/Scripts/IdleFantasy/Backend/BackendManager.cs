
namespace IdleFantasy {
    public static class BackendManager {

        public static IdleFantasyBackend Backend;

        public static void Init( IdleFantasyBackend i_backend ) {
            Backend = i_backend;
        }
    }
}
