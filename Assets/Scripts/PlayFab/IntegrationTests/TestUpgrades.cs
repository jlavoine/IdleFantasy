using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestUpgrades : IntegrationTestBase {
        private List<UpgradeTestData> mUpgradeTests;
        private UpgradeTestData mCurrentTestData;

        protected override IEnumerator RunAllTests() {
            yield return mBackend.WaitUntilNotBusy();

            SetTestData();

            foreach ( UpgradeTestData testData in mUpgradeTests ) {
                mCurrentTestData = testData;

                yield return Test_CanAffordUpgrade();
                yield return Test_CannotAffordUpgrade();
                yield return Test_CannotUpgradeAtMaxLevel();
            }

            DoneWithTests();
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

        private IEnumerator Test_CanAffordUpgrade() {
            SetPlayerData( mCurrentTestData.SaveKey, DrsStringUtils.Replace( mCurrentTestData.SaveValue, "NUM", 1 ) );
            SetPlayerCurrency( mCurrentTestData.Cost );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeUpgradeCall();

            FailTestIfCurrencyDoesNotEqual( 0 );
            FailTestIfNotProgressLevel( mCurrentTestData.TestClass, mCurrentTestData.TestID, 2 );

            yield return mBackend.WaitUntilNotBusy();
        }

        private IEnumerator Test_CannotUpgradeAtMaxLevel() {
            SetPlayerData( mCurrentTestData.SaveKey, DrsStringUtils.Replace( mCurrentTestData.SaveValue, "NUM", mCurrentTestData.MaxLevel ) );
            SetPlayerCurrency( 100000 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeUpgradeCall();

            FailTestIfClientInSync( "Test_CannotUpgradeAtMaxLevel" );
        }

        private IEnumerator Test_CannotAffordUpgrade() {
            SetPlayerData( mCurrentTestData.SaveKey, DrsStringUtils.Replace( mCurrentTestData.SaveValue, "NUM", 1 ) );
            SetPlayerCurrency( 0 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeUpgradeCall();

            FailTestIfClientInSync( "Test_CannotAffordUpgrade" );
        }

        private IEnumerator MakeUpgradeCall() {
            mBackend.MakeUpgradeCall( mCurrentTestData.TestClass, mCurrentTestData.TestID, mCurrentTestData.TestUpgradeID );
            yield return mBackend.WaitUntilNotBusy();
        }
    }
}