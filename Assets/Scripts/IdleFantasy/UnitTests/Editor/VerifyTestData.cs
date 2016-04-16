using System.Collections.Generic;
using NUnit.Framework;
using IdleFantasy.Data;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests {
    public class VerifyTestData {
        [Test]
        public void VerifyTestBuilding() {
            BuildingData testBuildingData = BuildingLoader.GetData( BuildingLoader.TEST_BUILDING );

            Assert.AreEqual( testBuildingData.ID, "TEST_BUILDING" );            
            Assert.AreEqual( testBuildingData.Size, 10 );            
            Assert.Contains( "TEST_CATEGORY", testBuildingData.Categories );

            Assert.AreEqual( testBuildingData.LevelUpgrade.MaxLevel, 50 );
            Assert.Contains( new KeyValuePair<string, int>( "Gold", 1000 ), testBuildingData.LevelUpgrade.ResourcesToUpgrade );
            Assert.AreEqual( "Level", testBuildingData.LevelUpgrade.PropertyName );            
        }

        [Test]
        public void VerifyTestUnit() {
            UnitData testUnitData = GenericDataLoader.GetData<UnitData>( GenericDataLoader.UNITS, GenericDataLoader.TEST_UNIT );

            Assert.AreEqual( "TEST_UNIT", testUnitData.ID );
            Assert.AreEqual( 1f, testUnitData.BaseProgressPerSecond );
        }
    }
}