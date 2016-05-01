
namespace MyLibrary {
    public interface IBackend  {
        void Authenticate();
        void SetUpCloudServices( bool i_testing );

        void GetTitleData( string i_key, Callback<string> requestSuccessCallback );
        void GetAllDataForClass( string i_className, Callback<string> requestSuccessCallback );

        bool IsBusy();
        //void RequestData();
    }
}