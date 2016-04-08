using System.Collections.Generic;
using NUnit.Framework;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests {
    public class BuildingUpgradeTests {

        static object[] BuildingLevels = {
            new object[] { 1 },
            new object[] { 0 },
            new object[] { 10 },
            new object[] { 22 },            
            new object[] { -10 },
            new object[] { 50 }
        };

        private int GetUpgradedLevel( Building i_building ) {
            int upgradedLevel = i_building.Level + 1;

            if (upgradedLevel > i_building.Data.MaxLevel) {
                upgradedLevel = i_building.Data.MaxLevel;
            } else if ( upgradedLevel < 1 ) {
                upgradedLevel = 1;
            }

            return upgradedLevel;
        }

        [Test]
        [TestCaseSource( "BuildingLevels" )]
        public void UpgradeTest( int i_level ) {            
            Building testBuilding = GetMockBuilding();
            testBuilding.Level = i_level;
            int expectedLevel = GetUpgradedLevel( testBuilding );

            testBuilding.Upgrade();
                      
            Assert.AreEqual( testBuilding.Level, expectedLevel );
        }

        [Test]
        public void UpgradeIncreasesCapacity() {            
            Building testBuilding = GetMockBuilding();
            int capacityBeforeUpgrade = testBuilding.Capacity;

            testBuilding.Upgrade();

            Assert.AreNotEqual( capacityBeforeUpgrade, testBuilding.Capacity );
        }

        [Test]
        [TestCaseSource( "BuildingLevels" )]
        public void VerifyUpgradeCosts( int i_level ) {
            Building testBuilding = GetMockBuilding();
            testBuilding.Level = i_level;

            NormalInventory inventory = new NormalInventory();
            foreach (KeyValuePair<string,int> cost in testBuilding.Data.ResourcesToUpgrade) {
                int amount = cost.Value * testBuilding.Level;
                inventory.SetResource( cost.Key, amount );
            }

            bool canAffordUpgrade = testBuilding.CanAffordUpgrade( inventory );

            Assert.IsTrue( canAffordUpgrade );
        }

        [Test]
        public void NoUpgradeAtMaxLevel() {
            Building testBuilding = GetMockBuilding();
            testBuilding.Level = testBuilding.Data.MaxLevel;

            bool canUpgrade = testBuilding.CanUpgrade( new FullInventory() );

            Assert.IsFalse( canUpgrade );
        }

        [Test]
        [TestCaseSource( "BuildingLevels" )]
        public void InitiateUpgrade_EnoughResources( int i_level ) {
            IResourceInventory fullInventory = new FullInventory();
            Building testBuilding = GetMockBuilding();
            testBuilding.Level = i_level;
            int expectedLevel = GetUpgradedLevel( testBuilding );

            testBuilding.InitiateUpgrade( fullInventory );
            
            Assert.AreEqual( testBuilding.Level, expectedLevel );
        }

        [Test]
        public void CanUpgrade_EnoughResources() {
            IResourceInventory fullInventory = new FullInventory();
            Building testBuilding = GetMockBuilding();

            bool canUpgrade = testBuilding.CanUpgrade( fullInventory );

            Assert.IsTrue( canUpgrade );
        }

        [Test]
        [TestCaseSource( "BuildingLevels" )]
        public void InitiateUpgrade_NotEnoughResource( int i_level ) {
            IResourceInventory emptyInventory = new EmptyInventory();
            Building testBuilding = GetMockBuilding();
            testBuilding.Level = i_level;

            int buildingLevelBeforeUpgrade = testBuilding.Level;
            testBuilding.InitiateUpgrade( emptyInventory );

            Assert.AreEqual( buildingLevelBeforeUpgrade, testBuilding.Level );
        }

        [Test]
        public void CanUpgrade_NotEnoughResources() {
            IResourceInventory emptyInventory = new EmptyInventory();
            Building testBuilding = GetMockBuilding();

            bool canUpgrade = testBuilding.CanUpgrade( emptyInventory );

            Assert.IsFalse( canUpgrade );
        }

        public static Building GetMockBuilding() {
            BuildingData data = GenericDataLoader.GetData<BuildingData>( GenericDataLoader.BUILDINGS, GenericDataLoader.TEST_BUILDING );
            return new Building( data, new MockUnit(1) );
        }
    }
}