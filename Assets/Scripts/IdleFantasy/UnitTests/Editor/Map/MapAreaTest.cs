using NUnit.Framework;
using MyLibrary;

#pragma warning disable 0414
namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class MapAreaTest {

        private MapArea mAreaUnderTest;
        private MapAreaData mMapAreaData;      

        [SetUp]
        public void BeforeTest() {
            UnitTestUtils.LoadOfflineData();

            OfflineBackend backend = (OfflineBackend) BackendManager.Backend;
            MapData mapData = backend.GetPlayerData_Offline<MapData>( BackendConstants.MAP_BASE );
            mMapAreaData = mapData.Areas[0];

            SingleMissionProgress mockMissionProgress = new SingleMissionProgress();
            mockMissionProgress.Completed = false;

            mAreaUnderTest = new MapArea( mMapAreaData, mockMissionProgress );
        }

        [Test]
        public void MapAreaTerrain_MatchesData() {
            string mapAreaTerrainProperty = mAreaUnderTest.ViewModel.GetPropertyValue<string>( MapViewProperties.TERRAIN_TYPE );
            Assert.AreEqual( mapAreaTerrainProperty, mMapAreaData.Terrain.ToString() );
        }
    }
}
