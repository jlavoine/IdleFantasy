using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class PlayFabManager : MonoBehaviour {

    private const string TITLE_ID = "B9C6";

    public string PlayFabId;

    void Start () {
        PlayFabSettings.TitleId = TITLE_ID;

        //ClientGetTitleData();

        Login( TITLE_ID );
        //Debug.Log( "HAI" );
	}

    public void ClientGetTitleData() {
        var getRequest = new GetTitleDataRequest();
        PlayFabClientAPI.GetTitleData( getRequest, ( result ) => {
            Debug.Log( "Got the following titleData:" );
            foreach ( var entry in result.Data ) {
                Debug.Log( entry.Key + ": " + entry.Value );
            }
        },
          ( error ) => {
              Debug.Log( "Got error getting titleData:" );
              Debug.Log( error.ErrorMessage );
          } );
    }

    private void Login( string i_titleID ) {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest() {
            TitleId = i_titleID,
            CreateAccount = true,
            CustomId = SystemInfo.deviceUniqueIdentifier
        };

        Debug.Log( "What's this: " + SystemInfo.deviceUniqueIdentifier );

        PlayFabClientAPI.LoginWithCustomID( request, ( result ) => {
            PlayFabId = result.PlayFabId;
            Debug.Log( "Got PlayFabID: " + PlayFabId );

            if ( result.NewlyCreated ) {
                Debug.Log( "(new account)" );
            }
            else {
                Debug.Log( "(existing account)" );
            }

            Debug.Log( "Here we go!" );
            //ClientGetTitleData();
            //SetUserData();
            GetUserData();
        },
        ( error ) => {
            Debug.Log( "Error logging in player with custom ID:" );
            Debug.Log( error.ErrorMessage );
        } );
    }

    private void SetUserData() {
        UpdateUserDataRequest request = new UpdateUserDataRequest() {
            Data = new Dictionary<string, string>(){
      //{"Ancestor", "Arthur"},
      {"Successor", "Douglas"}
    }
        };

        PlayFabClientAPI.UpdateUserData( request, ( result ) =>
        {
            Debug.Log( "Successfully updated user data" );
        }, ( error ) =>
        {
            Debug.Log( "Got error setting user data Ancestor to Arthur" );
            Debug.Log( error.ErrorDetails );
        } );
    }

    void GetUserData() {
        GetUserDataRequest request = new GetUserDataRequest() {
            PlayFabId = PlayFabId,
            Keys = null
        };

        PlayFabClientAPI.GetUserData( request, ( result ) => {
            Debug.Log( "Got user data:" );
            if ( ( result.Data == null ) || ( result.Data.Count == 0 ) ) {
                Debug.Log( "No user data available" );
            }
            else {
                foreach ( var item in result.Data ) {
                    Debug.Log( "    " + item.Key + " == " + item.Value.Value );
                }
            }
        }, ( error ) => {
            Debug.Log( "Got error retrieving user data:" );
            Debug.Log( error.ErrorMessage );
        } );
    }
}
