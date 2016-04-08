using System.Collections.Generic;
using NUnit.Framework;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests {
    public class VerifyTestData {
        [Test]
        public void VerifyTestBuilding() {
            BuildingData testBuildingData = BuildingLoader.GetData( BuildingLoader.TEST_BUILDING );

            Assert.AreEqual( testBuildingData.ID, "TEST_BUILDING" );            
            Assert.AreEqual( testBuildingData.Size, 10 );            
            Assert.Contains( "TEST_CATEGORY", testBuildingData.Categories );

            Assert.AreEqual( testBuildingData.Level.MaxLevel, 50 );
            Assert.Contains( new KeyValuePair<string, int>( "Gold", 1000 ), testBuildingData.Level.ResourcesToUpgrade );
            Assert.AreEqual( "Level", testBuildingData.Level.PropertyName );            
        }
    }
}