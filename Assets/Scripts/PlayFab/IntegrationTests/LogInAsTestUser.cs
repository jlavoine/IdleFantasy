using UnityEngine;
using MyLibrary;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class LogInAsTestUser : MonoBehaviour {
        public const string LOG_IN_DONE = "LogInComplete";

        public string ID;

        private IMessageService mMessenger;
        private IdleFantasyBackend mBackend;

        public void Start() {
            mMessenger = new MyMessenger();
            mBackend = new IdleFantasyBackend( mMessenger );
            BackendManager.Init( mBackend );

            mMessenger.AddListener<IAuthenticationSuccess>( BackendMessages.AUTH_SUCCESS, OnAuthenticationSucess );
            mMessenger.AddListener( BackendMessages.CLOUD_SETUP_SUCCESS, OnCloudSetupSuccess );
            mMessenger.AddListener<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, OnBackendFailure );

            mBackend.Authenticate( ID );
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
            mMessenger.Send<IBasicBackend>( LOG_IN_DONE, mBackend );
        }
    }
}