using NUnit.Framework;
using MyLibrary;
using NSubstitute;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class UnitUpgradeTests {

        private Building mBuilding;
        private Unit mUnit;

        [SetUp]
        public void BeforeTest() {
            UnitTestUtils.LoadOfflineData();
            UnitTestUtils.LoadMockPlayerData();

            UnitData data = GenericDataLoader.GetData<UnitData>( GenericDataLoader.TEST_UNIT );
            mUnit = new Unit( data,
                new UnitProgress() { Level = 1, Trainers = 1 },
                new ViewModel() );

            mBuilding = BuildingUpgradeTests.GetTestBuilding();
            mBuilding.Unit = mUnit;
        }


        [Test]
        public void UpgradeKeepsUnitProgress() {
            mBuilding.NextUnitProgress = .5f;

            mUnit.Level.Upgrade();

            Assert.AreEqual( .5f, mBuilding.NextUnitProgress );
        }

        [Test]
        public void UpgradeKeepsNumUnits() {
            mBuilding.NumUnits = 1;

            mUnit.Level.Upgrade();

            Assert.AreEqual( 1, mBuilding.NumUnits );
        }

        [Test]
        public void UpgradeSlowsDownUnitProgress() {

        }
    }
}