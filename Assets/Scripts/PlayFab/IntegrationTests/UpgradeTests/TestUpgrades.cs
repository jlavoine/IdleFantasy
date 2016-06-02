using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class TestUpgrades : IntegrationTestBase {
        protected List<UpgradeTestData> mUpgradeTests;
        protected UpgradeTestData mCurrentTestData;

        protected abstract IEnumerator RunTest();

        protected override IEnumerator RunAllTests() {
            SetTestData();

            foreach ( UpgradeTestData testData in mUpgradeTests ) {
                mCurrentTestData = testData;

                yield return RunTest();

                yield return mBackend.WaitUntilNotBusy();
            }
        }        

        private void SetTestData() {
            mUpgradeTests = new List<UpgradeTestData>();

            SetBuildingTestData();
            SetUnitTestData();
        }

        private void SetBuildingTestData() {
            UpgradeTestData testData = new UpgradeTestData();
            testData.SaveKey = "BuildingsProgress";
            testData.SaveValue = "{\"BASE_BUILDING_1\":{\"Level\":$NUM$}}";
            testData.TestID = "BASE_BUILDING_1";
            testData.TestClass = "Buildings";
            testData.TestUpgradeID = "BuildingLevel";
            testData.MaxLevel = 50;
            testData.Cost = 1000;
            mUpgradeTests.Add( testData );
        }

        private void SetUnitTestData() {
            UpgradeTestData testData = new UpgradeTestData();
            testData.SaveKey = "UnitsProgress";
            testData.SaveValue = "{\"BASE_MELEE_1\":{\"Level\":$NUM$}}";
            testData.TestID = "BASE_MELEE_1";
            testData.TestClass = "Units";
            testData.TestUpgradeID = "UnitLevel";
            testData.MaxLevel = 50;
            testData.Cost = 1000;
            mUpgradeTests.Add( testData );
        }

        protected IEnumerator MakeUpgradeCall() {
            mBackend.MakeUpgradeCall( mCurrentTestData.TestClass, mCurrentTestData.TestID, mCurrentTestData.TestUpgradeID );
            yield return mBackend.WaitUntilNotBusy();
        }
    }
}