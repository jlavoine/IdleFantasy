using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

namespace MyLibrary {
    public class PlayFabBackend : IBackend {
        private const string TITLE_ID = "B9C6";

        private IMessageService mMessenger;

        public string PlayFabId;

        public PlayFabBackend( IMessageService i_messenger ) {
            mMessenger = i_messenger;
        }

        public void Authenticate() {
            PlayFabSettings.TitleId = TITLE_ID;

            LoginWithCustomIDRequest request = new LoginWithCustomIDRequest() {
                TitleId = TITLE_ID,
                CreateAccount = true,
                CustomId = SystemInfo.deviceUniqueIdentifier
            };

            PlayFabClientAPI.LoginWithCustomID( request, ( result ) => {
                PlayFabId = result.PlayFabId;
                IAuthenticationSuccess successResult = null;
                mMessenger.Send<IAuthenticationSuccess>( BackendMessages.AUTH_SUCCESS, successResult );
            },
            ( error ) => {
                IBackendFailure failureResult = null;
                mMessenger.Send<IBackendFailure>( BackendMessages.AUTH_FAIL, failureResult );
            } );
        }

        public void SetUpCloudServices( bool i_testing ) {
            GetCloudScriptUrlRequest request = new GetCloudScriptUrlRequest() {
                Testing = i_testing
            };

            PlayFabClientAPI.GetCloudScriptUrl( request, ( result ) => {
                mMessenger.Send( BackendMessages.CLOUD_SETUP_SUCCESS );
            },
            ( error ) => {
                IBackendFailure failure = null;
                mMessenger.Send<IBackendFailure>( BackendMessages.CLOUD_SETUP_FAIL, failure );
            } );
        }
    }
}