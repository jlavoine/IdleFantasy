using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestUpgrades : MonoBehaviour {

        void Start() {
            StartCoroutine( RunAllTests() );            
        }

        private IEnumerator RunAllTests() {
            while ( BackendManager.Backend.IsBusy() ) {
                yield return 0;
            }

            Dictionary<string, string> setDataParams = new Dictionary<string, string>();
            setDataParams["Key"] = "BuildingsProgress";
            setDataParams["Value"] = "{\"BASE_BUILDING_1\":{\"Level\":1}}";
            BackendManager.Backend.MakeCloudCall( "setPlayerData", setDataParams, null );

            Dictionary<string, string> setCurrencyParams = new Dictionary<string, string>();
            setCurrencyParams["Type"] = VirtualCurrencies.GOLD;
            setCurrencyParams["Amount"] = "1000";
            BackendManager.Backend.MakeCloudCall( "setPlayerCurrency", setCurrencyParams, null );

            while ( BackendManager.Backend.IsBusy() ) {
                yield return 0;
            }

            IntegrationTest.Pass();
        }

        private void OnDone( Dictionary<string,string> i_results ) {
            IntegrationTest.Pass();
        }
    }
}