using NUnit.Framework;
using MyLibrary;
using IdleFantasy.UnitTests.Units;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests.Buildings {
    [TestFixture]
    public class WorldResetBuildingTests {
        private Building mBuildingUnderTest;

        [SetUp]
        public void BeforeTests() {
            UnitTestUtils.LoadOfflineData();
            UnitTestUtils.LoadMockPlayerData();

            mBuildingUnderTest = BuildingUpgradeTests.GetTestBuilding();
        }

        [TearDown]
        public void AfterTests() {
            mBuildingUnderTest.Dispose();
            EasyMessenger.Instance = null;
        }

        [Test]
        public void BuildingLevelResets_AfterWorldReset() {            
            mBuildingUnderTest.Level.Value = 10;

            EasyMessenger.Instance.Send( MapKeys.WORLD_RESET_SUCCESS );

            Assert.AreEqual( 1, mBuildingUnderTest.Level.Value );
        }

        [Test]
        public void BuildingNumUnitsResets_AfterWorldReset() {
            mBuildingUnderTest.NumUnits = 1000;

            EasyMessenger.Instance.Send( MapKeys.WORLD_RESET_SUCCESS );

            Assert.AreEqual( 0, mBuildingUnderTest.NumUnits );
        }

        [Test]
        public void BuildingUnitLevelResets_AfterWorldReset() {
            mBuildingUnderTest.Unit.Level.Value = 10;

            EasyMessenger.Instance.Send( MapKeys.WORLD_RESET_SUCCESS );

            Assert.AreEqual( 1, mBuildingUnderTest.Unit.Level.Value );
        }

        [Test]
        public void BuildingUnitTrainingResets_AfterWorldReset() {
            mBuildingUnderTest.Unit.TrainingLevel = 1;

            EasyMessenger.Instance.Send( MapKeys.WORLD_RESET_SUCCESS );

            Assert.AreEqual( 0, mBuildingUnderTest.Unit.TrainingLevel );
        }
    }
}
