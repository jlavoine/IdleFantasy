using UnityEngine;
using MyLibrary;

namespace IdleFantasy {
    public class Login : MonoBehaviour {

        private IBackend mBackend;

        void Start() {
            Messenger.AddListener<IAuthenticationSuccess>( BackendMessages.AUTH_SUCCESS, OnAuthenticationSucess );
            Messenger.AddListener< IBackendFailure>( BackendMessages.AUTH_SUCCESS, OnAuthenticationFailure );

            Messenger.AddListener( BackendMessages.AUTH_SUCCESS, OnCloudSetupSuccess );
            Messenger.AddListener<IBackendFailure>( BackendMessages.AUTH_SUCCESS, OnCloudSetupFailure );

            IMessageService messageService = new MyMessenger();
            mBackend = new PlayFabBackend( messageService );
        }

        void OnDestroy() {
            Messenger.RemoveListener<IAuthenticationSuccess>( BackendMessages.AUTH_SUCCESS, OnAuthenticationSucess );
            Messenger.RemoveListener<IBackendFailure>( BackendMessages.AUTH_SUCCESS, OnAuthenticationFailure );

            Messenger.RemoveListener( BackendMessages.AUTH_SUCCESS, OnCloudSetupSuccess );
            Messenger.RemoveListener<IBackendFailure>( BackendMessages.AUTH_SUCCESS, OnCloudSetupFailure );
        }

        private void OnAuthenticationSucess( IAuthenticationSuccess i_success ) {
            mBackend.SetUpCloudServices( false );
        }

        private void OnAuthenticationFailure( IBackendFailure i_failure ) {

        }

        private void OnCloudSetupSuccess() {

        }

        private void OnCloudSetupFailure( IBackendFailure i_failure ) {

        }
    }
}