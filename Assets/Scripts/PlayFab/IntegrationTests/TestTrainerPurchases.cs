using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestTrainerPurchases : MonoBehaviour {
        private string SAVE_KEY = "TrainerSaveData";
        private string SAVE_VALUE = "{\"TrainerCounts\":{\"Normal\":$NUM$},\"TrainerAssignments\":{}}";

        private int COST = 2000;

        private IdleFantasyBackend mBackend;

        void Start() {
            mBackend = BackendManager.Backend;
            StartCoroutine( RunAllTests() );
        }

        private IEnumerator RunAllTests() {
            yield return mBackend.WaitUntilNotBusy();

            yield return Test_CanAffordNewTrainer();
            yield return Test_CannotAffordNewTrainer();

            DoneWithTests();
        }

        private IEnumerator Test_CanAffordNewTrainer() {
            SetTrainers( 1 );
            SetPlayerCurrency( COST );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakePurchaseCall();

            FailTestIfCurrencyDoesNotEqual( 0 );
            FailTestIfNotTrainerCount( 2 );

            yield return mBackend.WaitUntilNotBusy();
        }

        private void FailTestIfNotTrainerCount( int i_count ) {
            mBackend.MakeCloudCall( "getTrainerCount", null, ( results ) => {
                if ( results.ContainsKey( "data" ) ) {
                    int count = int.Parse( results["data"] );
                    if ( count != i_count ) {
                        IntegrationTest.Fail( "Count did not match: " + i_count );
                    }
                }
                else {
                    IntegrationTest.Fail( "Results did not have data." );
                }
            } );
        }

        private void FailTestIfCurrencyDoesNotEqual( int i_amount ) {
            mBackend.GetVirtualCurrency( VirtualCurrencies.GOLD, ( numGold ) => {
                if ( numGold != i_amount ) {
                    IntegrationTest.Fail( "Currency did not equal " + i_amount );
                }
            } );
        }

        private IEnumerator Test_CannotAffordNewTrainer() {
            SetTrainers( 1 );
            SetPlayerCurrency( 0 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakePurchaseCall();

            FailTestIfClientInSync( "Test_CannotNewTrainer" );
        }

        private void SetTrainers( int i_level ) {
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

        private IEnumerator MakePurchaseCall() {
            mBackend.MakeTrainerPurchase();
            yield return mBackend.WaitUntilNotBusy();
        }

        private void FailTestIfClientInSync( string i_testName ) {
            if ( !mBackend.ClientOutOfSync ) {
                IntegrationTest.Fail( i_testName + ": Client should be out of sync, but it's not." );
            }
        }

        private void DoneWithTests() {
            IntegrationTest.Pass();
        }
    }
}