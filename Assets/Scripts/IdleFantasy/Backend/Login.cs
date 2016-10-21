using MyLibrary;
using UnityEngine;
using System.Collections.Generic;

namespace IdleFantasy {
    public class Login {

        private IBackend mBackend;

        private IAnalyticsTimer mLoginTimer;

        public Login( IBackend i_backend, IAnalyticsTimer i_loginTimer ) {
            mBackend = i_backend;

            mLoginTimer = i_loginTimer;
            mLoginTimer.Start();
        }

        public void Start() {                        
            MyMessenger.AddListener<IAuthenticationSuccess>( BackendMessages.AUTH_SUCCESS, OnAuthenticationSucess );            
            MyMessenger.AddListener( BackendMessages.CLOUD_SETUP_SUCCESS, OnCloudSetupSuccess );
            MyMessenger.AddListener<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, OnBackendFailure );

            mLoginTimer.Start();

            mBackend.Authenticate( SystemInfo.deviceUniqueIdentifier );
            //mBackend.Authenticate( TestUsers.FOUR );
        }

        public void OnDestroy() {
            MyMessenger.RemoveListener<IAuthenticationSuccess>( BackendMessages.AUTH_SUCCESS, OnAuthenticationSucess );
            MyMessenger.RemoveListener( BackendMessages.CLOUD_SETUP_SUCCESS, OnCloudSetupSuccess );
            MyMessenger.RemoveListener<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, OnBackendFailure );
        }

        private void OnAuthenticationSucess( IAuthenticationSuccess i_success ) {
            mLoginTimer.StepComplete( LibraryAnalyticEvents.AUTH_TIME );

            MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Authenticate success", "" );

            mBackend.SetUpCloudServices( false );
        }

        private void OnCloudSetupSuccess() {
            mLoginTimer.StepComplete( LibraryAnalyticEvents.CLOUD_SETUP_TIME );

            MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Cloud setup success", "" );

            mBackend.MakeCloudCall( "onLogin", null, OnLogin );
        }

        private void OnLogin( Dictionary<string, string> i_result ) {
            mLoginTimer.StepComplete( LibraryAnalyticEvents.ON_LOGIN_TIME );
            IdleFantasyBackend backend = (IdleFantasyBackend) mBackend;
            backend.SetLoggedInTime();

            MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Login success", "" );

            MyMessenger.Send( BackendMessages.LOGIN_SUCCESS );
        }

        private void OnBackendFailure( IBackendFailure i_failure ) {
            MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, i_failure.GetMessage(), "" );          
            mLoginTimer.StopTimer();            
        }       
    }
}