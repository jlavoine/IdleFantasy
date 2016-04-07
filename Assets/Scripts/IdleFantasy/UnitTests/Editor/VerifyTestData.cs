using System.Collections.Generic;
using NUnit.Framework;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests {
    public class VerifyTestData {
        [Test]
        public void VerifyTestBuilding() {
            BuildingData testBuildingData = BuildingLoader.GetData( BuildingLoader.TEST_BUILDING );

            Assert.AreEqual( testBuildingData.ID, "TEST_BUILDING" );
            Assert.AreEqual( testBuildingData.MaxLevel, 50 );
            Assert.AreEqual( testBuildingData.Size, 10 );
            Assert.Contains( new KeyValuePair<string, int>( "Wood", 10), testBuildingData.ResourcesToUpgrade );
            Assert.Contains( "TEST_CATEGORY", testBuildingData.Categories );
        }
    }
}