using MyLibrary;

namespace IdleFantasy {
    public class Login {

        private IMessageService mMessenger;
        private IBackend mBackend;
        private ILogService mLogger;

        private AnalyticsTimer mLoginTimer;

        public Login( IMessageService i_messenger, ILogService i_logger, IBackend i_backend ) {
            mMessenger = i_messenger;
            mLogger = i_logger;
            mBackend = i_backend;

            mLoginTimer = new AnalyticsTimer( i_messenger, AnalyticEvents.LOGIN_TIME );
        }

        public void Start() {                        
            mMessenger.AddListener<IAuthenticationSuccess>( BackendMessages.AUTH_SUCCESS, OnAuthenticationSucess );
            mMessenger.AddListener< IBackendFailure>( BackendMessages.AUTH_FAIL, OnAuthenticationFailure );

            mMessenger.AddListener( BackendMessages.CLOUD_SETUP_SUCCESS, OnCloudSetupSuccess );
            mMessenger.AddListener<IBackendFailure>( BackendMessages.CLOUD_SETUP_FAIL, OnCloudSetupFailure );

            mLoginTimer.Start();
            mBackend.Authenticate();
        }

        void OnDestroy() {
            mMessenger.RemoveListener<IAuthenticationSuccess>( BackendMessages.AUTH_SUCCESS, OnAuthenticationSucess );
            mMessenger.RemoveListener<IBackendFailure>( BackendMessages.AUTH_FAIL, OnAuthenticationFailure );

            mMessenger.RemoveListener( BackendMessages.CLOUD_SETUP_SUCCESS, OnCloudSetupSuccess );
            mMessenger.RemoveListener<IBackendFailure>( BackendMessages.CLOUD_SETUP_FAIL, OnCloudSetupFailure );
        }

        private void OnAuthenticationSucess( IAuthenticationSuccess i_success ) {
            mLogger.Log( LogTypes.Info, "Authenticate success" );

            mBackend.SetUpCloudServices( false );
        }

        private void OnAuthenticationFailure( IBackendFailure i_failure ) {
            mLogger.Log( LogTypes.Info, "Authenticate failure" );

            mLoginTimer.Stop();
        }

        private void OnCloudSetupSuccess() {
            mLogger.Log( LogTypes.Info, "Cloud setup success" );

            mLoginTimer.StopAndSend();

            mMessenger.Send( BackendMessages.LOGIN_SUCCESS );
        }

        private void OnCloudSetupFailure( IBackendFailure i_failure ) {
            mLogger.Log( LogTypes.Info, "Cloud setup failure" );

            mLoginTimer.Stop();
        }
    }
}