using NUnit.Framework;
using MyLibrary;

#pragma warning disable 0414
namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class MapAreaTest {

        private MapArea mAreaUnderTest;
        private MapAreaData mMapAreaData;
        private SingleMissionProgress mMissionProgress;    

        [SetUp]
        public void BeforeTest() {
            UnitTestUtils.LoadOfflineData();

            OfflineBackend backend = (OfflineBackend) BackendManager.Backend;
            MapData mapData = backend.GetPlayerData_Offline<MapData>( BackendConstants.MAP_BASE );
            mMapAreaData = mapData.Areas[0];

            mMissionProgress = new SingleMissionProgress();
            mMissionProgress.Completed = false;

            mAreaUnderTest = new MapArea( mMapAreaData, mMissionProgress );
        }

        [Test]
        public void MapAreaTerrain_MatchesData() {
            string mapAreaTerrainProperty = mAreaUnderTest.ViewModel.GetPropertyValue<string>( MapViewProperties.TERRAIN_TYPE );
            Assert.AreEqual( mapAreaTerrainProperty, mMapAreaData.Terrain.ToString() );
        }

        [Test]
        public void MapAreaCompletion_MatchesData() {
            bool mapAreaCompletion = mAreaUnderTest.ViewModel.GetPropertyValue<bool>( MapViewProperties.AREA_COMPLETED );
            Assert.AreEqual( mapAreaCompletion, mMissionProgress.Completed );
        }

        [Test]
        public void WhenMissionCompleted_AreaNotAccessible() {
            mMissionProgress.Completed = true;
            mAreaUnderTest = new MapArea( mMapAreaData, mMissionProgress );

            bool isAccessible = mAreaUnderTest.ViewModel.GetPropertyValue<bool>( MapViewProperties.AREA_ACCESS );

            Assert.IsFalse( isAccessible );
        }

        [Test]
        public void WhenMissionNotCompleted_AreaAccessible() {
            mMissionProgress.Completed = false;
            mAreaUnderTest = new MapArea( mMapAreaData, mMissionProgress );

            bool isAccessible = mAreaUnderTest.ViewModel.GetPropertyValue<bool>( MapViewProperties.AREA_ACCESS );

            Assert.IsTrue( isAccessible );
        }
    }
}
