using MyLibrary;

namespace IdleFantasy.UnitTests {
    public static class UnitTestUtils {
        private static IBasicBackend mOfflineBackend;

        public static void LoadOfflineData() {
            mOfflineBackend = new OfflineBackend();

            LoadConstants();
            LoadGenericData();
        }

        private static void LoadConstants() {
            Constants.Init( mOfflineBackend );
        }

        private static void LoadGenericData() {
            GenericDataLoader.Init( mOfflineBackend );
            GenericDataLoader.LoadDataOfClass<BuildingData>( GenericDataLoader.BUILDINGS );
            GenericDataLoader.LoadDataOfClass<UnitData>( GenericDataLoader.UNITS );
            GenericDataLoader.LoadDataOfClass<GuildData>( GenericDataLoader.GUILDS );
        }
    }
}