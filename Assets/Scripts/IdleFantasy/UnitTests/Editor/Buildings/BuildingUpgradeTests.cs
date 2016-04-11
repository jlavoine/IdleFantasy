using NUnit.Framework;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests {
    public class BuildingUpgradeTests {

        [Test]
        public void UpgradeIncreasesCapacity() {            
            Building testBuilding = GetMockBuilding();
            int capacityBeforeUpgrade = testBuilding.Capacity;

            testBuilding.Level.Upgrade();

            Assert.AreNotEqual( capacityBeforeUpgrade, testBuilding.Capacity );
        }

        public static Building GetMockBuilding() {
            BuildingData data = GenericDataLoader.GetData<BuildingData>( GenericDataLoader.BUILDINGS, GenericDataLoader.TEST_BUILDING );
            return new Building( data, new MockUnit(1) );
        }
    }
}