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
            ( error ) => { HandleError( error, BackendMessages.AUTH_FAIL ); } );
        }

        public void SetUpCloudServices( bool i_testing ) {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Starting cloud service setup call", PLAYFAB );

            GetCloudScriptUrlRequest request = new GetCloudScriptUrlRequest() {
                Testing = i_testing
            };

            PlayFabClientAPI.GetCloudScriptUrl( request, ( result ) => {
                mMessenger.Send( BackendMessages.CLOUD_SETUP_SUCCESS );
            },
            ( error ) => { HandleError( error, BackendMessages.CLOUD_SETUP_FAIL ); } );
        }

        public void GetTitleData( string i_key, Callback<string> requestSuccessCallback ) {
            StartRequest( "Requesting title data for " + i_key );

            GetTitleDataRequest request = new GetTitleDataRequest() {
                Keys = new List<string>() { i_key }
            };

            PlayFabClientAPI.GetTitleData( request, ( result ) => {
                RequestComplete( "Request title data success for " + i_key, LogTypes.Info );

                // should only call the callback ONCE because there is only one key
                foreach ( var entry in result.Data ) {
                    requestSuccessCallback(entry.Value);
                }                
            },
            ( error ) => { HandleError( error, BackendMessages.TITLE_DATA_FAIL ); } );
        }

        public void GetPlayerData( string i_key, Callback<string> requestSuccessCallback ) {
            StartRequest( "Request player data " + i_key );
            
            GetUserDataRequest request = new GetUserDataRequest() {
                PlayFabId = PlayFabId,
                Keys = new List<string>() { i_key }
            };

            PlayFabClientAPI.GetUserReadOnlyData( request, ( result ) => {
                RequestComplete( "Player data request complete: " + i_key, LogTypes.Info );

                if ( ( result.Data == null ) || ( result.Data.Count == 0 ) ) {
                    mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "No user data for " + i_key, PLAYFAB );
                }
                else {
                    // should only call the callback ONCE because there is only one key
                    foreach ( var item in result.Data ) {
                        requestSuccessCallback( item.Value.Value );
                    }
                }
            }, ( error ) => { HandleError( error, BackendMessages.PLAYER_DATA_REQUEST_FAIL ); } );
        }

        public void GetVirtualCurrency( string i_key, Callback<int> requestSuccessCallback ) {
            StartRequest( "Requesting virtual currency: " + i_key );

            GetUserCombinedInfoRequest request = new GetUserCombinedInfoRequest();

            PlayFabClientAPI.GetUserCombinedInfo( request, ( result ) => {
                RequestComplete( "Request for virtual currency complete: " + i_key, LogTypes.Info );

                int currency = 0;
                if ( result.VirtualCurrency.ContainsKey( i_key ) ) {
                    currency = result.VirtualCurrency[i_key];
                } else {
                    mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "No virtual currency for: " + i_key, PLAYFAB );
                }

                requestSuccessCallback( currency );
            },
            ( error ) => { HandleError( error, BackendMessages.CURRENCY_REQUEST_FAIL ); } );
        }

        public void GetAllTitleDataForClass( string i_className, Callback<string> requestSuccessCallback ) {
            StartRequest( "Request all data for class " + i_className );

            Dictionary<string, string> upgradeParams = new Dictionary<string, string>();
            upgradeParams.Add( "Class", i_className );

            RunCloudScriptRequest request = new RunCloudScriptRequest() {
                ActionId = "getAllDataForClass",
                Params = new { data = upgradeParams }
            };

            PlayFabClientAPI.RunCloudScript( request, ( result ) => {
                RequestComplete( "Cloud logs for all data request for " + i_className + ": " + result.ActionLog, LogTypes.Info );

                if ( result.Results != null ) {
                    string res = result.Results.ToString();
                    res = res.CleanStringForJsonDeserialization();

                    requestSuccessCallback( res );
                }
            }, ( error ) => { HandleError( error, BackendMessages.TITLE_DATA_FAIL ); } );
        }

        private void HandleError( PlayFabError i_error, string i_messageType ) {
            RequestComplete( "Backend failure!", LogTypes.Error );

            IBackendFailure failure = null;
            mMessenger.Send<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, failure );
            mMessenger.Send<IBackendFailure>( i_messageType, failure );
        }

        private void StartRequest( string i_message ) {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, i_message, PLAYFAB );
            CloudRequestCount++;
        }

        private void RequestComplete( string i_message, LogTypes i_messageType ) {
            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, i_messageType, i_message, PLAYFAB );
            CloudRequestCount--;
        }
    }
}