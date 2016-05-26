using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class IntegrationTestBase : MonoBehaviour {
        protected abstract IEnumerator RunAllTests();

        protected IdleFantasyBackend mBackend;

        void Start() {
            mBackend = BackendManager.Backend;
            StartCoroutine( RunAllTests() );
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
    }
}