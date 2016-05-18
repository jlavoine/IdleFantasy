using System.Collections.Generic;
using NUnit.Framework;
using IdleFantasy.Data;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class VerifyTestData {

        [SetUp]
        public void BeforeTest() {
            UnitTestUtils.LoadOfflineData();
        }

        [Test]
        public void VerifyTestBuilding() {
            BuildingData testBuildingData = GenericDataLoader.GetData<BuildingData>( GenericDataLoader.BUILDINGS, GenericDataLoader.TEST_BUILDING );

            Assert.AreEqual( testBuildingData.ID, "BASE_BUILDING_1" );            
            Assert.AreEqual( testBuildingData.StartingSize, 10 );            
            Assert.Contains( "TEST_CATEGORY", testBuildingData.Categories );

            Assert.AreEqual( testBuildingData.Level.MaxLevel, 50 );
            Assert.Contains( new KeyValuePair<string, int>( "Gold", 1000 ), testBuildingData.Level.ResourcesToUpgrade );
            Assert.AreEqual( "Level", testBuildingData.Level.PropertyName );            
        }

        [Test]
        public void VerifyTestUnit() {
            UnitData testUnitData = GenericDataLoader.GetData<UnitData>( GenericDataLoader.UNITS, GenericDataLoader.TEST_UNIT );

            Assert.AreEqual( "BASE_UNIT_1", testUnitData.ID );
            Assert.AreEqual( 1f, testUnitData.BaseProgressPerSecond );
        }
    }
}