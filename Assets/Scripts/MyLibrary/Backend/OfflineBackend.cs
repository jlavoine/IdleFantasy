using System;
using UnityEngine;
using MyLibrary.PlayFab;    // this is here just for string clean up/deserialization since I just copy the json on the playfab servers for offline usage (unit tests)

namespace MyLibrary {
    public class OfflineBackend : IBackend {
        public void Authenticate() {
            throw new NotImplementedException();
        }

        public void GetAllTitleDataForClass( string i_className, Callback<string> requestSuccessCallback ) {
            string filePath = Application.streamingAssetsPath + "/OfflineData/" + i_className + ".json";
            string data = DataUtils.LoadFileWithPath( filePath );
            data = data.CleanStringForJsonDeserialization();
            requestSuccessCallback( data );
        }

        public void GetPlayerData( string i_key, Callback<string> requestSuccessCallback ) {
            throw new NotImplementedException();
        }

        public void GetTitleData( string i_key, Callback<string> requestSuccessCallback ) {
            throw new NotImplementedException();
        }

        public void GetVirtualCurrency( string i_key, Callback<int> requetSuccessCallback ) {
            throw new NotImplementedException();
        }

        public bool IsBusy() {
            throw new NotImplementedException();
        }

        public void SetUpCloudServices( bool i_testing ) {
            throw new NotImplementedException();
        }
    }
}