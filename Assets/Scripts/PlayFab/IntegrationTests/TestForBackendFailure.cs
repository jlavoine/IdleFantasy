using UnityEngine;
using System.Collections;
using MyLibrary;

// The purpose of this integration test:
// 1) Check general login flow to the PlayFab service
// 2) Make all calls that would be made during gameplay (accessing currencies, player data, etc) to make sure none result in an error
// TODO: These tests need to be improved...if the actual data is not there, the test still passes because there is no actual error on PlayFab's side.
// So basically right now this test just checks that PlayFab is working properly.

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestForBackendFailure : MonoBehaviour {

        private IMessageService mMessenger;
        private IBackend mBackend;

        void Start() {
            mMessenger = new MyMessenger();
            mMessenger.AddListener<IBackend>( LogInAsTestUser.LOG_IN_DONE, LogInComplete );            
        }

        void OnDestroy() {
            mMessenger.RemoveListener<IBackend>( LogInAsTestUser.LOG_IN_DONE, LogInComplete );
        }    

        private void LogInComplete( IBackend i_backend ) {
            mBackend = i_backend;

            StartCoroutine( RunAllTests() );
        }

        private IEnumerator RunAllTests() {
            StartTests();

            while ( mBackend.IsBusy() ) {
                yield return 0;
            }

            IntegrationTest.Pass();
        }

        private void StartTests() {
            TestCurrencyCalls();
            TestPlayerDataCalls();
            TestTitleDataCalls();
            TestAllTitleDataCalls();            
        }

        private void TestCurrencyCalls() {
            mBackend.GetVirtualCurrency( VirtualCurrencies.GOLD, ( result ) => { } );
        }

        private void TestPlayerDataCalls() {
            mBackend.GetPlayerData( PlayerData.BUILDING_PROGRESS, ( result ) => { } );
            mBackend.GetPlayerData( PlayerData.UNIT_PROGRESS, ( result ) => { } );
        }

        private void TestTitleDataCalls() {
            mBackend.GetTitleData( "StringTable_English", ( result ) => { } );
        }

        private void TestAllTitleDataCalls() {
            mBackend.GetAllTitleDataForClass( GenericDataLoader.BUILDINGS, ( result ) => { } );
            mBackend.GetAllTitleDataForClass( GenericDataLoader.UNITS, ( result ) => { } );
        }
    }
}