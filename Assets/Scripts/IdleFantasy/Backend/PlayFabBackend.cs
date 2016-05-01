using UnityEngine;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

namespace MyLibrary {
    public class PlayFabBackend : IBackend {
        private const string TITLE_ID = "B9C6";
        public const string PLAYFAB = "PlayFab";

        private IMessageService mMessenger;

        private int mCloudRequestCount = 0;
        public int CloudRequestCount {
            get { return mCloudRequestCount; }
            set { mCloudRequestCount = value; }
        }

        public string PlayFabId;

        public PlayFabBackend( IMessageService i_messenger ) {
            mMessenger = i_messenger;
        }

        public bool IsBusy() {
            return mCloudRequestCount > 0;
        }

        public void Authenticate() {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Authentication attempt for title " + TITLE_ID, PLAYFAB );

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
                mMessenger.Send<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, failureResult );
            } );
        }

        public void SetUpCloudServices( bool i_testing ) {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Starting cloud service setup call", PLAYFAB );

            GetCloudScriptUrlRequest request = new GetCloudScriptUrlRequest() {
                Testing = i_testing
            };

            PlayFabClientAPI.GetCloudScriptUrl( request, ( result ) => {
                mMessenger.Send( BackendMessages.CLOUD_SETUP_SUCCESS );
            },
            ( error ) => {
                IBackendFailure failure = null;
                mMessenger.Send<IBackendFailure>( BackendMessages.CLOUD_SETUP_FAIL, failure );
                mMessenger.Send<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, failure );
            } );
        }

        public void GetTitleData( string i_key, Callback<string> requestSuccessCallback ) {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Requesting title data for " + i_key, PLAYFAB );

            CloudRequestCount++;

            GetTitleDataRequest request = new GetTitleDataRequest() {
                Keys = new List<string>() { i_key }
            };

            PlayFabClientAPI.GetTitleData( request, ( result ) => {
                CloudRequestCount--;

                mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Request title data success for " + i_key, PLAYFAB );

                foreach ( var entry in result.Data ) {
                    requestSuccessCallback(entry.Value);
                }                
            },
              ( error ) => {
                  CloudRequestCount--;

                  IBackendFailure failure = null;
                  mMessenger.Send<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, failure );
              } );
        }
    }
}