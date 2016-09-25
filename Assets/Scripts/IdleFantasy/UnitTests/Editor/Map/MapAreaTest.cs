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

        [TearDown]
        public void AfterTest() {
            mAreaUnderTest.Dispose();
        }

        private void RecreateArea() {
            mAreaUnderTest.Dispose();
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
        public void WhenMissionIsComplete_AreaNotAccessible() {
            mMissionProgress.Completed = true;
            RecreateArea();

            bool isAccessible = mAreaUnderTest.ViewModel.GetPropertyValue<bool>( MapViewProperties.AREA_ACCESS );

            Assert.IsFalse( isAccessible );
        }

        [Test]
        public void WhenMissionIsNotComplete_AreaAccessible() {
            mMissionProgress.Completed = false;
            RecreateArea();

            bool isAccessible = mAreaUnderTest.ViewModel.GetPropertyValue<bool>( MapViewProperties.AREA_ACCESS );

            Assert.IsTrue( isAccessible );
        }

        [Test]
        public void WhenMissionGetsComplete_WithAreaIndex_AreaIsCompleteAndInaccessible() {
            mMissionProgress.Completed = false;
            RecreateArea();

            MissionData testData = new MissionData();
            testData.Index = mAreaUnderTest.Data.Index;
            MyMessenger.Send( MissionKeys.MISSION_COMPLETED, testData );

            bool isAccessible = mAreaUnderTest.ViewModel.GetPropertyValue<bool>( MapViewProperties.AREA_ACCESS );
            bool isComplete = mAreaUnderTest.ViewModel.GetPropertyValue<bool>( MapViewProperties.AREA_COMPLETED );

            Assert.IsTrue( isComplete );
            Assert.IsFalse( isAccessible );
        }

        [Test]
        public void WhenMissionGetsComplete_NotWithAreaIndex_NothingChanges() {
            mMissionProgress.Completed = false;
            RecreateArea();

            MissionData testData = new MissionData();
            testData.Index = mAreaUnderTest.Data.Index+1;
            MyMessenger.Send( MissionKeys.MISSION_COMPLETED, testData );

            bool isAccessible = mAreaUnderTest.ViewModel.GetPropertyValue<bool>( MapViewProperties.AREA_ACCESS );
            bool isComplete = mAreaUnderTest.ViewModel.GetPropertyValue<bool>( MapViewProperties.AREA_COMPLETED );

            Assert.IsFalse( isComplete );
            Assert.IsTrue( isAccessible );
        }
    }
}
