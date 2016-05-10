using UnityEngine;
using System.Collections;

namespace MyLibrary.PlayFab.IntegrationTests {
    public class TestForBackendFailure : MonoBehaviour {

        private IMessageService mMessenger;
        private IBackend mBackend;

        public void Start() {
            mMessenger = new MyMessenger();
            mBackend = new PlayFabBackend( mMessenger );

            mMessenger.AddListener<IAuthenticationSuccess>( BackendMessages.AUTH_SUCCESS, OnAuthenticationSucess );
            mMessenger.AddListener( BackendMessages.CLOUD_SETUP_SUCCESS, OnCloudSetupSuccess );
            mMessenger.AddListener<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, OnBackendFailure );

            mBackend.Authenticate();
        }

        void OnDestroy() {
            mMessenger.RemoveListener<IAuthenticationSuccess>( BackendMessages.AUTH_SUCCESS, OnAuthenticationSucess );
            mMessenger.RemoveListener( BackendMessages.CLOUD_SETUP_SUCCESS, OnCloudSetupSuccess );
            mMessenger.RemoveListener<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, OnBackendFailure );
        }

        private void OnAuthenticationSucess( IAuthenticationSuccess i_success ) {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Authenticate success", "" );
            mBackend.SetUpCloudServices( false );
        }

        private void OnBackendFailure( IBackendFailure i_failure ) {            
            IntegrationTest.Fail( i_failure.GetMessage() );
        }

        private void OnCloudSetupSuccess() {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Cloud setup success", "" );

            StartCoroutine(RunAllTests());
        }

        private IEnumerator RunAllTests() {
            StartTests();

            while ( mBackend.IsBusy() ) {
                yield return 0;
            }

            IntegrationTest.Pass();
        }

        private void StartTests() {
            mBackend.GetVirtualCurrency( "NA", ( result ) => {} );
            mBackend.GetAllTitleDataForClass( "NA", ( result ) => { } );
            mBackend.GetPlayerData( "NA", ( result ) => { } );
            mBackend.GetTitleData( "NA", ( result ) => { } );
        }
    }
}