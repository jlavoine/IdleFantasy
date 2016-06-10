using MyLibrary;
using UnityEngine;
using System.Collections.Generic;

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
            mMessenger.AddListener( BackendMessages.CLOUD_SETUP_SUCCESS, OnCloudSetupSuccess );
            mMessenger.AddListener<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, OnBackendFailure );

            mLoginTimer.Start();
            mBackend.Authenticate( SystemInfo.deviceUniqueIdentifier );
        }

        public void OnDestroy() {
            mMessenger.RemoveListener<IAuthenticationSuccess>( BackendMessages.AUTH_SUCCESS, OnAuthenticationSucess );
            mMessenger.RemoveListener( BackendMessages.CLOUD_SETUP_SUCCESS, OnCloudSetupSuccess );
            mMessenger.RemoveListener<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, OnBackendFailure );
        }

        private void OnAuthenticationSucess( IAuthenticationSuccess i_success ) {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Authenticate success", "" );

            mBackend.SetUpCloudServices( false );
        }

        private void OnCloudSetupSuccess() {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Cloud setup success", "" );

            mBackend.MakeCloudCall( "onLogin", null, OnLogin );
        }

        private void OnLogin( Dictionary<string, string> i_result ) {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Login success", "" );

            mLoginTimer.StopAndSend();

            mMessenger.Send( BackendMessages.LOGIN_SUCCESS );
        }

        private void OnBackendFailure( IBackendFailure i_failure ) {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, i_failure.GetMessage(), "" );          
            mLoginTimer.Stop();            
        }       
    }
}