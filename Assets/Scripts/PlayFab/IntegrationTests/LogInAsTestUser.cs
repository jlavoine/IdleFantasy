using UnityEngine;
using MyLibrary;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class LogInAsTestUser : MonoBehaviour {
        public const string LOG_IN_DONE = "LogInComplete";

        public string ID;

        private IdleFantasyBackend mBackend;

        public void Start() {
            mBackend = new IdleFantasyBackend();
            BackendManager.Init( mBackend );

            MyMessenger.AddListener<IAuthenticationSuccess>( BackendMessages.AUTH_SUCCESS, OnAuthenticationSucess );
            MyMessenger.AddListener( BackendMessages.CLOUD_SETUP_SUCCESS, OnCloudSetupSuccess );
            MyMessenger.AddListener<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, OnBackendFailure );

            mBackend.Authenticate( ID );
        }

        void OnDestroy() {
            MyMessenger.RemoveListener<IAuthenticationSuccess>( BackendMessages.AUTH_SUCCESS, OnAuthenticationSucess );
            MyMessenger.RemoveListener( BackendMessages.CLOUD_SETUP_SUCCESS, OnCloudSetupSuccess );
            MyMessenger.RemoveListener<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, OnBackendFailure );
        }

        private void OnAuthenticationSucess( IAuthenticationSuccess i_success ) {
            MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Authenticate success", "" );
            mBackend.SetUpCloudServices( false );
        }

        private void OnBackendFailure( IBackendFailure i_failure ) {
            IntegrationTest.Fail( i_failure.GetMessage() );
        }

        private void OnCloudSetupSuccess() {
            MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Cloud setup success", "" );
            MyMessenger.Send<IBasicBackend>( LOG_IN_DONE, mBackend );
        }
    }
}