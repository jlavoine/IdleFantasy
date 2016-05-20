﻿using System.Collections.Generic;

namespace MyLibrary {
    public interface IBasicBackend  {
        void Authenticate( string i_id );
        void SetUpCloudServices( bool i_testing );

        void MakeCloudCall( string i_methodName, Dictionary<string,string> i_params, Callback<Dictionary<string, string>> requestSuccessCallback );

        void GetTitleData( string i_key, Callback<string> requestSuccessCallback );
        void GetAllTitleDataForClass( string i_className, Callback<string> requestSuccessCallback );
        void GetPlayerData( string i_key, Callback<string> requestSuccessCallback );
        void GetVirtualCurrency( string i_key, Callback<int> requetSuccessCallback );

        bool IsClientOutOfSync();
        bool IsBusy();
    }
}