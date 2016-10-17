using System.Collections.Generic;
using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class TestStandardUpgrades : TestUpgrades {

        protected override void SetTestData() {
            mUpgradeTests = new List<UpgradeTestData>();

            SetBuildingTestData();
            SetUnitTestData();
        }

        private void SetBuildingTestData() {
            UpgradeTestData testData = new UpgradeTestData();
            testData.SaveKey = "BuildingsProgress";
            testData.SaveValue = "{\"BASE_WARRIOR_BUILDING_1\":{\"Level\":$NUM$}}";
            testData.TestID = "BASE_WARRIOR_BUILDING_1";
            testData.TestClass = "Buildings";
            testData.TestUpgradeID = "BuildingLevel";
            testData.MaxLevel = 50;
            testData.Cost = 500;
            mUpgradeTests.Add( testData );
        }

        private void SetUnitTestData() {
            UpgradeTestData testData = new UpgradeTestData();
            testData.SaveKey = "UnitsProgress";
            testData.SaveValue = "{\"BASE_WARRIOR_1\":{\"Level\":$NUM$}}";
            testData.TestID = "BASE_WARRIOR_1";
            testData.TestClass = "Units";
            testData.TestUpgradeID = "UnitLevel";
            testData.MaxLevel = 50;
            testData.Cost = 500;
            mUpgradeTests.Add( testData );
        }

        protected IEnumerator MakeUpgradeCall() {
            mBackend.MakeUpgradeCall( mCurrentTestData.TestClass, mCurrentTestData.TestID, mCurrentTestData.TestUpgradeID );
            yield return mBackend.WaitUntilNotBusy();
        }
    }
}