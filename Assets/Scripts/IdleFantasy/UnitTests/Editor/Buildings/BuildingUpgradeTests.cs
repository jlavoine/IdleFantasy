using NUnit.Framework;

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
            Building testBuilding = GetTestBuilding();
            int capacityBeforeUpgrade = testBuilding.Capacity;

            testBuilding.Level.Upgrade();

            Assert.AreNotEqual( capacityBeforeUpgrade, testBuilding.Capacity );
        }

        public static Building GetTestBuilding() {
            return new Building( new BuildingProgress() { ID = GenericDataLoader.TEST_BUILDING, Level = 1 }, new UnitProgress() { ID = GenericDataLoader.TEST_UNIT, Level = 1, Trainers = 1 } );
        }
    }
}