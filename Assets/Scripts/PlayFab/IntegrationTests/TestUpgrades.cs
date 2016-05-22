using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestUpgrades : MonoBehaviour {

        private string SAVE_KEY = "BuildingsProgress";
        private string SAVE_VALUE = "{\"BASE_BUILDING_1\":{\"Level\":$NUM$}}";
        private string TEST_ID = "BASE_BUILDING_1";
        private string TEST_CLASS = "Buildings";
        private string TEST_UPGRADE_ID = "Level";

        private int MAX_LEVEL = 50;
        private int COST = 1000;

        private IdleFantasyBackend mBackend;

        void Start() {
            mBackend = BackendManager.Backend;
            StartCoroutine( RunAllTests() );            
        }

        private IEnumerator RunAllTests() {
            yield return mBackend.WaitUntilNotBusy();

            yield return Test_CanAffordUpgrade();
            yield return Test_CannotAffordUpgrade();
            yield return Test_CannotUpgradeAtMaxLevel();

            DoneWithTests();
        }

        private IEnumerator Test_CanAffordUpgrade() {
            SetPlayerData( 1 );
            SetPlayerCurrency( COST );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeUpgradeCall();

            FailTestIfCurrencyDoesNotEqual( 0 );
            FailTestIfNotProgressLevel( 2 );

            yield return mBackend.WaitUntilNotBusy();
        }

        private IEnumerator Test_CannotUpgradeAtMaxLevel() {
            SetPlayerData( MAX_LEVEL );
            SetPlayerCurrency( 100000 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeUpgradeCall();

            FailTestIfClientInSync( "Test_CannotUpgradeAtMaxLevel" );
        }

        private IEnumerator Test_CannotAffordUpgrade() {
            SetPlayerData( 1 );
            SetPlayerCurrency( 0 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeUpgradeCall();

            FailTestIfClientInSync( "Test_CannotAffordUpgrade" );
        }

        private IEnumerator MakeUpgradeCall() {
            mBackend.MakeUpgradeCall( TEST_CLASS, TEST_ID, TEST_UPGRADE_ID );
            yield return mBackend.WaitUntilNotBusy();
        }

        private void FailTestIfClientInSync( string i_testName ) {
            if ( !mBackend.ClientOutOfSync ) {
                IntegrationTest.Fail( i_testName +  ": Client should be out of sync, but it's not." );
            }
        }

        private void FailTestIfNotProgressLevel( int i_level ) {
            Dictionary<string, string> getParams = new Dictionary<string, string>();
            getParams.Add( "Class", TEST_CLASS );
            getParams.Add( "TargetID", TEST_ID );

            mBackend.MakeCloudCall( "getProgressData", getParams, ( results ) => {
                if ( results.ContainsKey( "data" ) ) {
                    BuildingProgress progress = JsonConvert.DeserializeObject<BuildingProgress>( results["data"] );
                    if ( progress.Level != i_level ) {
                        IntegrationTest.Fail( "Level did not match: " + i_level );
                    }
                } else {
                    IntegrationTest.Fail( "Results did not have data." );
                }
            } );
        }

        private void FailTestIfCurrencyDoesNotEqual( int i_amount ) {
            mBackend.GetVirtualCurrency( VirtualCurrencies.GOLD, ( numGold ) => {
                if ( numGold != i_amount ) {
                    IntegrationTest.Fail("Currency did not equal " + i_amount);
                }
            } );
        }

        private void SetPlayerData( int i_level ) {
            Dictionary<string, string> setDataParams = new Dictionary<string, string>();
            setDataParams["Key"] = SAVE_KEY;
            setDataParams["Value"] = DrsStringUtils.Replace( SAVE_VALUE, "NUM", i_level );
            mBackend.MakeCloudCall( IdleFantasyBackend.TEST_SET_DATA, setDataParams, null );
        }

        private void SetPlayerCurrency( int i_amount ) {
            Dictionary<string, string> setCurrencyParams = new Dictionary<string, string>();
            setCurrencyParams["Type"] = VirtualCurrencies.GOLD;
            setCurrencyParams["Amount"] = i_amount.ToString();
            mBackend.MakeCloudCall( IdleFantasyBackend.TEST_SET_CURRENCY, setCurrencyParams, null );
        }

        private void DoneWithTests() {
            IntegrationTest.Pass();
        }
    }
}