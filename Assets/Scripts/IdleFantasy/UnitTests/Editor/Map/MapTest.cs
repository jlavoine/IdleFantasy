using NUnit.Framework;
using MyLibrary;

#pragma warning disable 0414
namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class MapTest {

        private Map mMapUnderTest;
        private MapData mMapData;

        [SetUp]
        public void BeforeTest() {
            UnitTestUtils.LoadOfflineData();
            OfflineBackend backend = (OfflineBackend) BackendManager.Backend;
            mMapData = backend.GetPlayerData_Offline<MapData>( BackendConstants.MAP_BASE );
            mMapUnderTest = new Map( mMapData );
        }

        [Test]
        public void MapNameProperty_MatchesMapData() {
            string mapNameProperty = mMapUnderTest.ViewModel.GetPropertyValue<string>( MapViewProperties.NAME );
            Assert.AreEqual( mapNameProperty, mMapData.Name.GetStringName() );
        }
    }
}
