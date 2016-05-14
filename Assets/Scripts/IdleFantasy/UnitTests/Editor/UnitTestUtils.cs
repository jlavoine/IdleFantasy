using MyLibrary;

namespace IdleFantasy.UnitTests {
    public static class UnitTestUtils {

        public static void LoadOfflineData() {
            GenericDataLoader.Init( new OfflineBackend(), new MyMessenger() );
            GenericDataLoader.LoadDataOfClass<BuildingData>( GenericDataLoader.BUILDINGS );
            GenericDataLoader.LoadDataOfClass<UnitData>( GenericDataLoader.UNITS );
        }
    }
}