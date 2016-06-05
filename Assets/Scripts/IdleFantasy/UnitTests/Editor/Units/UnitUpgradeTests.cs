using NUnit.Framework;
using MyLibrary;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class UnitUpgradeTests {

        private Building mBuilding;
        private Unit mUnit;

        [SetUp]
        public void BeforeTest() {
            UnitTestUtils.LoadOfflineData();

            UnitData data = GenericDataLoader.GetData<UnitData>( GenericDataLoader.UNITS, GenericDataLoader.TEST_UNIT );
            mUnit = new Unit( GenericDataLoader.GetData<UnitData>( GenericDataLoader.UNITS, GenericDataLoader.TEST_UNIT ),
                new UnitProgress() { Level = 1, Trainers = 1 },
                new ViewModel() );

            mBuilding = BuildingUpgradeTests.GetMockBuilding();
            mBuilding.Unit = mUnit;
        }


        [Test]
        public void UpgradeResetsNextUnitProgress() {
            mBuilding.NextUnitProgress = .5f;

            mUnit.Level.Upgrade();

            Assert.AreEqual( 0, mBuilding.NextUnitProgress );
        }

        [Test]
        public void UpgradeResetsNumUnits() {
            mBuilding.NumUnits = 1;

            mUnit.Level.Upgrade();

            Assert.AreEqual( 0, mBuilding.NumUnits );
        }

        [Test]
        public void UpgradeSlowsDownUnitProgress() {

        }
    }
}