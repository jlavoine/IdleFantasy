using MyLibrary;

namespace IdleFantasy.UnitTests {
    public static class UnitTestUtils {
        private static IBasicBackend mOfflineBackend;
        private static IMessageService mOfflineMessenger;

        public static void LoadOfflineData() {
            mOfflineBackend = new OfflineBackend();
            mOfflineMessenger = new MyMessenger();

            LoadConstants();
            LoadGenericData();
        }

        private static void LoadConstants() {
            Constants.Init( mOfflineBackend, mOfflineMessenger );
        }

        private static void LoadGenericData() {
            GenericDataLoader.Init( mOfflineBackend, mOfflineMessenger );
            GenericDataLoader.LoadDataOfClass<BuildingData>( GenericDataLoader.BUILDINGS );
            GenericDataLoader.LoadDataOfClass<UnitData>( GenericDataLoader.UNITS );
        }
    }
}