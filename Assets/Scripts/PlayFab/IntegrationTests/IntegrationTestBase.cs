using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class IntegrationTestBase : MonoBehaviour {
        protected abstract IEnumerator RunAllTests();

        protected IdleFantasyBackend mBackend;

        void Start() {
            mBackend = BackendManager.Backend;
            StartCoroutine( StartTests() );
        }

        private IEnumerator StartTests() {
            yield return mBackend.WaitUntilNotBusy();

            yield return RunAllTests();

            DoneWithTests();
        }

        protected void SetPlayerData( string i_key, string i_value ) {
            Dictionary<string, string> setDataParams = new Dictionary<string, string>();
            setDataParams["Key"] = i_key;
            setDataParams["Value"] = i_value;
            mBackend.MakeCloudCall( IdleFantasyBackend.TEST_SET_DATA, setDataParams, null );
        }

        protected void SetPlayerCurrency( int i_amount ) {
            Dictionary<string, string> setCurrencyParams = new Dictionary<string, string>();
            setCurrencyParams["Type"] = VirtualCurrencies.GOLD;
            setCurrencyParams["Amount"] = i_amount.ToString();
            mBackend.MakeCloudCall( IdleFantasyBackend.TEST_SET_CURRENCY, setCurrencyParams, null );
        }

        protected void DoneWithTests() {
            IntegrationTest.Pass();
        }

        protected void FailTestIfClientInSync( string i_testName ) {
            if ( !mBackend.ClientOutOfSync ) {
                IntegrationTest.Fail( i_testName + ": Client should be out of sync, but it's not." );
            }
        }

        protected void FailTestIfCurrencyDoesNotEqual( int i_amount ) {
            mBackend.GetVirtualCurrency( VirtualCurrencies.GOLD, ( numGold ) => {
                if ( numGold != i_amount ) {
                    IntegrationTest.Fail( "Currency did not equal " + i_amount );
                }
            } );
        }

        protected void FailTestIfNotProgressLevel( string i_class, string i_targetID, int i_level ) {
            Dictionary<string, string> getParams = new Dictionary<string, string>();
            getParams.Add( "Class", i_class );
            getParams.Add( "TargetID", i_targetID );

            mBackend.MakeCloudCall( "getProgressData", getParams, ( results ) => {
                if ( results.ContainsKey( "data" ) ) {
                    ProgressBase progress = JsonConvert.DeserializeObject<ProgressBase>( results["data"] );
                    if ( progress.Level != i_level ) {
                        IntegrationTest.Fail( "Level did not match: " + i_level );
                    }
                }
                else {
                    IntegrationTest.Fail( "Results did not have data." );
                }
            } );
        }

        protected void FailTestIfReturnedCallDoesNotEqual( string i_cloudMethod, int i_count ) {
            mBackend.MakeCloudCall( i_cloudMethod, null, ( results ) => {
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
    }
}