
namespace MyLibrary {
    public interface IBackend  {
        void Authenticate();
        void SetUpCloudServices( bool i_testing );

        void GetTitleData( string i_key, Callback<string> requestSuccessCallback );
        void GetAllTitleDataForClass( string i_className, Callback<string> requestSuccessCallback );
        void GetPlayerData( string i_key, Callback<string> requestSuccessCallback );
        void GetVirtualCurrency( string i_key, Callback<int> requetSuccessCallback );

        bool IsBusy();
    }
}