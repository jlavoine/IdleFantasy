using System;
using UnityEngine;
using MyLibrary.PlayFab;    // this is here just for string clean up/deserialization since I just copy the json on the playfab servers for offline usage (unit tests)
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;

namespace MyLibrary {
    public class OfflineBackend : IBasicBackend {
        public void Authenticate( string i_id ) {
            throw new NotImplementedException();
        }

        public void GetAllTitleDataForClass( string i_className, Callback<string> requestSuccessCallback ) {
            string filePath = Application.streamingAssetsPath + "/OfflineData/" + i_className + ".json";
            string data = GetCleanTextAtPath( filePath );
            requestSuccessCallback( data );
        }

        public void GetPlayerData( string i_key, Callback<string> requestSuccessCallback ) {
            string filePath = Application.streamingAssetsPath + "/OfflineData/PlayerData/" + i_key + ".json";
            string data = GetCleanTextAtPath( filePath );
            requestSuccessCallback( data );
        }

        public void GetTitleData( string i_key, Callback<string> requestSuccessCallback ) {
            if ( i_key == Constants.TITLE_DATA_KEY ) {
                string filePath = Application.streamingAssetsPath + "/OfflineData/Constants.json";
                string data = GetCleanTextAtPath( filePath );
                requestSuccessCallback( data );
            }
        }

        public void GetPlayerDataDeserialized<T>( string i_key, Callback<T> requestSuccessCallback ) {
            requestSuccessCallback( GetPlayerData_Offline<T>( i_key ) );
        }

        public T GetPlayerData_Offline<T>( string i_key ) {
            string filePath = Application.streamingAssetsPath + "/OfflineData/Player/" + i_key + ".json";
            string data = GetCleanTextAtPath( filePath );
            return JsonConvert.DeserializeObject<T>( data );
        }

        public void GetVirtualCurrency( string i_key, Callback<int> requetSuccessCallback ) {
            throw new NotImplementedException();
        }

        public bool IsBusy() {
            throw new NotImplementedException();
        }

        public bool IsClientOutOfSync() {
            throw new NotImplementedException();
        }

        public void MakeCloudCall( string i_methodName, Dictionary<string, string> i_params, Callback<Dictionary<string, string>> requestSuccessCallback ) {
            throw new NotImplementedException();
        }

        public void ResetSyncState() {
            throw new NotImplementedException();
        }

        public void SetUpCloudServices( bool i_testing ) {
            throw new NotImplementedException();
        }

        public IEnumerator WaitForCloudCall( string i_methodName, Dictionary<string, string> i_params, Callback<Dictionary<string, string>> requestSuccessCallback ) {
            throw new NotImplementedException();
        }

        public IEnumerator WaitUntilNotBusy() {
            throw new NotImplementedException();
        }

        private string GetCleanTextAtPath( string i_path ) {
            string data = DataUtils.LoadFileWithPath( i_path );
            data = data.CleanStringForJsonDeserialization();

            return data;
        }

        public void RestartClient() {
            throw new NotImplementedException();
        }
    }
}