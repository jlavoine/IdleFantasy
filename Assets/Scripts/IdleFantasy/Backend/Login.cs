using MyLibrary;
using UnityEngine;
using System.Collections;

namespace IdleFantasy {
    public class Login {

        private IMessageService mMessenger;
        private IBackend mBackend;

        private AnalyticsTimer mLoginTimer;

        public Login( IMessageService i_messenger, IBackend i_backend ) {
            mMessenger = i_messenger;

            mBackend = i_backend;

            mLoginTimer = new AnalyticsTimer( i_messenger, LibraryAnalyticEvents.LOGIN_TIME );
        }

        public void Start() {                        
            mMessenger.AddListener<IAuthenticationSuccess>( BackendMessages.AUTH_SUCCESS, OnAuthenticationSucess );
            mMessenger.AddListener<IBackendFailure>( BackendMessages.AUTH_FAIL, OnAuthenticationFailure );

            mMessenger.AddListener( BackendMessages.CLOUD_SETUP_SUCCESS, OnCloudSetupSuccess );
            mMessenger.AddListener<IBackendFailure>( BackendMessages.CLOUD_SETUP_FAIL, OnCloudSetupFailure );

            mLoginTimer.Start();
            mBackend.Authenticate( SystemInfo.deviceUniqueIdentifier );
        }

        void OnDestroy() {
            mMessenger.RemoveListener<IAuthenticationSuccess>( BackendMessages.AUTH_SUCCESS, OnAuthenticationSucess );
            mMessenger.RemoveListener<IBackendFailure>( BackendMessages.AUTH_FAIL, OnAuthenticationFailure );

            mMessenger.RemoveListener( BackendMessages.CLOUD_SETUP_SUCCESS, OnCloudSetupSuccess );
            mMessenger.RemoveListener<IBackendFailure>( BackendMessages.CLOUD_SETUP_FAIL, OnCloudSetupFailure );
        }

        private void OnAuthenticationSucess( IAuthenticationSuccess i_success ) {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Authenticate success", "" );

            mBackend.SetUpCloudServices( false );
        }

        private void OnAuthenticationFailure( IBackendFailure i_failure ) {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Authenticate failure", "" );

            mLoginTimer.Stop();
        }

        private void OnCloudSetupSuccess() {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Cloud setup success", "" );

            mLoginTimer.StopAndSend();

            mMessenger.Send( BackendMessages.LOGIN_SUCCESS );
        }

        private void OnCloudSetupFailure( IBackendFailure i_failure ) {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Cloud setup failure", "" );

            mLoginTimer.Stop();
        }       
    }
}