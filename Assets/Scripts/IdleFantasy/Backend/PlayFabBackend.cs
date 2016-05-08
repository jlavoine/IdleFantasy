using UnityEngine;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using MyLibrary.PlayFab;
using Newtonsoft.Json;

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

                // should only call the callback ONCE because there is only one key
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

        public void GetPlayerData( string i_key, Callback<string> requestSuccessCallback ) {
            CloudRequestCount++;

            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Request player data " + i_key, PLAYFAB );

            GetUserDataRequest request = new GetUserDataRequest() {
                PlayFabId = PlayFabId,
                Keys = new List<string>() { i_key }
            };

            PlayFabClientAPI.GetUserReadOnlyData( request, ( result ) => {
                CloudRequestCount--;

                if ( ( result.Data == null ) || ( result.Data.Count == 0 ) ) {
                    mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "No user data for " + i_key, PLAYFAB );
                }
                else {
                    // should only call the callback ONCE because there is only one key
                    foreach ( var item in result.Data ) {
                        requestSuccessCallback( item.Value.Value );
                    }
                }
            }, ( error ) => {
                CloudRequestCount--;

                IBackendFailure failure = null;
                mMessenger.Send<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, failure );
            } );
        }

        public void GetVirtualCurrency( string i_key, Callback<int> requestSuccessCallback ) {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Requesting virtual currency: " + i_key, PLAYFAB );

            CloudRequestCount++;

            GetUserCombinedInfoRequest request = new GetUserCombinedInfoRequest();

            PlayFabClientAPI.GetUserCombinedInfo( request, ( result ) => {
                mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Request for virtual currency complete: " + i_key, PLAYFAB );

                CloudRequestCount--;

                int currency = 0;
                if ( result.VirtualCurrency.ContainsKey( i_key ) ) {
                    currency = result.VirtualCurrency[i_key];
                } else {
                    mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "No virtual currency for: " + i_key, PLAYFAB );
                }

                requestSuccessCallback( currency );
            },
            ( error ) => {
                CloudRequestCount--;

                IBackendFailure failure = null;
                mMessenger.Send<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, failure );
            } );
        }

        public void GetAllTitleDataForClass( string i_className, Callback<string> requestSuccessCallback ) {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Request all data for class " + i_className, PLAYFAB );

            CloudRequestCount++;

            Dictionary<string, string> upgradeParams = new Dictionary<string, string>();
            upgradeParams.Add( "Class", i_className );

            RunCloudScriptRequest request = new RunCloudScriptRequest() {
                ActionId = "getAllDataForClass",
                Params = new { data = upgradeParams }
            };

            PlayFabClientAPI.RunCloudScript( request, ( result ) => {
                mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Cloud logs for all data request for " + i_className + ": " + result.ActionLog, PLAYFAB );

                CloudRequestCount--;

                if ( result.Results != null ) {
                    string res = result.Results.ToString();
                    res = res.CleanStringForJsonDeserialization();

                    requestSuccessCallback( res );
                }
            }, ( error ) => {
                CloudRequestCount--;

                IBackendFailure failure = null;
                mMessenger.Send<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, failure );
            } );
        }      
    }
}