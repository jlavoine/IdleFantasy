using System.Collections.Generic;
using NUnit.Framework;
using MyLibrary;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class UpgradeableTests {
        static object[] Levels = {
            new object[] { 1 },
            new object[] { 0 },
            new object[] { 10 },
            new object[] { 22 },
            new object[] { -10 },
            new object[] { 50 }
        };

        private ViewModel mTestModel;
        private Upgradeable mUpgrade;

        private int GetUpgradedLevel( IUpgradeable i_upgrade ) {
            int upgradedLevel = i_upgrade.Value + 1;

            if ( upgradedLevel > i_upgrade.MaxLevel ) {
                upgradedLevel = i_upgrade.MaxLevel;
            }
            else if ( upgradedLevel < 1 ) {
                upgradedLevel = 1;
            }

            return upgradedLevel;
        }

        [SetUp]
        public void BeforeTest() {
            UnitTestUtils.LoadOfflineData();
            mTestModel = new ViewModel();

            UpgradeData testUpgradeData = new UpgradeData();
            testUpgradeData.MaxLevel = 50;
            testUpgradeData.PropertyName = "Level";
            testUpgradeData.ResourcesToUpgrade = new Dictionary<string, int>() { { "Gold", 1000 } };

            mUpgrade = new Upgradeable();
            mUpgrade.SetPropertyToUpgrade( mTestModel, testUpgradeData );
        }

        [Test]
        [TestCaseSource( "Levels" )]
        public void UpgradeTest( int i_level ) {
            mTestModel.SetProperty( "Level", i_level );           
            int expectedLevel = GetUpgradedLevel( mUpgrade );

            mUpgrade.Upgrade();

            Assert.AreEqual( mUpgrade.Value, expectedLevel );
        }

        [Test]
        [TestCaseSource( "Levels" )]
        public void VerifyUpgradeCosts( int i_level ) {
            mUpgrade.Value = i_level;

            NormalInventory inventory = new NormalInventory();
            foreach ( KeyValuePair<string, int> cost in mUpgrade.ResourcesToUpgrade ) {
                int amount = cost.Value * mUpgrade.Value;
                inventory.SetResource( cost.Key, amount );
            }

            bool canAffordUpgrade = mUpgrade.CanAffordUpgrade( inventory );

            Assert.IsTrue( canAffordUpgrade );
        }

        [Test]
        [TestCaseSource( "Levels" )]
        public void VerifyUpgradeSpendDeduction( int i_level ) {
            mUpgrade.Value = i_level;

            NormalInventory inventory = new NormalInventory();
            foreach ( KeyValuePair<string, int> cost in mUpgrade.ResourcesToUpgrade ) {
                int amount = cost.Value * mUpgrade.Value;
                inventory.SetResource( cost.Key, amount );
            }

            if ( mUpgrade.CanUpgrade( inventory ) ) {
                mUpgrade.InitiateUpgrade( inventory );

                foreach ( KeyValuePair<string, int> cost in mUpgrade.ResourcesToUpgrade ) {
                    string resource = cost.Key;
                    int remainingAmount = inventory.GetResourceCount( resource );
                    Assert.AreEqual( 0, remainingAmount );
                }
            }
        }

        [Test]
        [TestCaseSource( "Levels" )]
        public void InitiateUpgrade_EnoughResources( int i_level ) {
            IResourceInventory fullInventory = new FullInventory();
            mUpgrade.Value = i_level;
            int expectedLevel = GetUpgradedLevel( mUpgrade );

            mUpgrade.InitiateUpgrade( fullInventory );

            Assert.AreEqual( mUpgrade.Value, expectedLevel );
        }

        [Test]
        public void CanUpgrade_EnoughResources() {
            IResourceInventory fullInventory = new FullInventory();

            bool canUpgrade = mUpgrade.CanUpgrade( fullInventory );

            Assert.IsTrue( canUpgrade );
        }

        [Test]
        [TestCaseSource( "Levels" )]
        public void InitiateUpgrade_NotEnoughResource( int i_level ) {
            IResourceInventory emptyInventory = new EmptyInventory();
            mUpgrade.Value = i_level;

            int levelBeforeUpgrade = mUpgrade.Value;
            mUpgrade.InitiateUpgrade( emptyInventory );

            Assert.AreEqual( levelBeforeUpgrade, mUpgrade.Value );
        }

        [Test]
        public void CanUpgrade_NotEnoughResources() {
            IResourceInventory emptyInventory = new EmptyInventory();

            bool canUpgrade = mUpgrade.CanUpgrade( emptyInventory );

            Assert.IsFalse( canUpgrade );
        }

        [Test]
        public void NoUpgradeAtMaxLevel() {
            mUpgrade.Value = mUpgrade.MaxLevel;

            bool canUpgrade = mUpgrade.CanUpgrade( new FullInventory() );

            Assert.IsFalse( canUpgrade );
        }
    }
}