using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class IntegrationTestBase : MonoBehaviour {        
        protected abstract IEnumerator RunAllTests();

        protected IIdleFantasyBackend mBackend;

        void Start() {
            mBackend = BackendManager.Backend;
            StartCoroutine( StartTests() );
        }

        private IEnumerator StartTests() {
            yield return mBackend.WaitUntilNotBusy();

            yield return RunAllTests();

            DoneWithTests();
        }

        protected void DoneWithTests() {
            IntegrationTest.Pass();
        }

        protected void FailTestIfClientInSync( string i_testName ) {
            if ( !mBackend.IsClientOutOfSync() ) {
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

        protected void GetProgressData<T>( string i_class, string i_targetID, Callback<T> i_resultsCallback ) {
            Dictionary<string, string> getParams = new Dictionary<string, string>();
            getParams.Add( "Class", i_class );
            getParams.Add( "TargetID", i_targetID );

            mBackend.MakeCloudCall( CloudTestMethods.getProgressData.ToString(), getParams, ( results ) => {
                if ( results.ContainsKey( "data" ) ) {
                    T progress = JsonConvert.DeserializeObject<T>( results["data"] );
                    i_resultsCallback( progress );
                }
                else {
                    IntegrationTest.Fail( "Results did not have data." );
                }
            } );
        }

        protected IEnumerator FailTestIfNotProgressLevel<T>( string i_class, string i_targetID, int i_level ) where T : ProgressBase {
            GetProgressData<T>( i_class, i_targetID, ( result ) => {
                if ( result.Level != i_level ) {
                    IntegrationTest.Fail( "Level did not match: " + i_level );
                }
            } );

            yield return mBackend.WaitUntilNotBusy();
        }

        protected IEnumerator GetNumberFromCloudCall( string i_cloudMethod, Dictionary<string,string> i_params, Callback<double> i_callback) {
            mBackend.MakeCloudCall( i_cloudMethod, i_params, ( results ) => {
                if ( results.ContainsKey( "data" ) ) {
                    double value = double.Parse( results["data"] );
                    i_callback( value );
                }
                else {
                    IntegrationTest.Fail( "Results did not have data." );
                }
            } );

            yield return mBackend.WaitUntilNotBusy();
        }

        protected void FailTestIfReturnedCallDoesNotEqual( string i_cloudMethod, double i_value, Dictionary<string,string> i_params = null ) {
            mBackend.MakeCloudCall( i_cloudMethod, i_params, ( results ) => {
                if ( results.ContainsKey( "data" ) ) {
                    double value = double.Parse( results["data"] );

                    if ( value != i_value ) {
                        IntegrationTest.Fail( "Value should have been " + i_value + " but was " + value );
                    }
                }
                else {
                    IntegrationTest.Fail( "Results did not have data." );
                }
            } );
        }

        protected void FailTestIfReturnedCallEquals( string i_cloudMethod, double i_value, Dictionary<string, string> i_params = null ) {
            mBackend.MakeCloudCall( i_cloudMethod, i_params, ( results ) => {
                if ( results.ContainsKey( "data" ) ) {
                    double value = double.Parse( results["data"] );
                    
                    if ( value == i_value ) {
                        IntegrationTest.Fail( "Value was: " + i_value );
                    }
                }
                else {
                    IntegrationTest.Fail( "Results did not have data." );
                }
            } );
        }
    }
}