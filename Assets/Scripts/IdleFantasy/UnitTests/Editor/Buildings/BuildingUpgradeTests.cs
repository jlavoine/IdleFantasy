using NUnit.Framework;
using MyLibrary;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class BuildingUpgradeTests {

        [SetUp]
        public void BeforeTest() {
            UnitTestUtils.LoadOfflineData();
        }

        [Test]
        public void UpgradeIncreasesCapacity() {            
            Building testBuilding = GetMockBuilding();
            int capacityBeforeUpgrade = testBuilding.Capacity;

            testBuilding.Level.Upgrade();

            Assert.AreNotEqual( capacityBeforeUpgrade, testBuilding.Capacity );
        }

        public static Building GetMockBuilding() {
            BuildingData data = GenericDataLoader.GetData<BuildingData>( GenericDataLoader.BUILDINGS, GenericDataLoader.TEST_BUILDING );
            return new Building( data, new BuildingProgress() { Level = 1 }, new UnitProgress() { Level = 1, Trainers = 1 } );
        }
    }
}