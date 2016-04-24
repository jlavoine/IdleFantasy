
namespace MyLibrary {
    public interface IBackend  {
        void Authenticate();
        void SetUpCloudServices( bool i_testing );
        //void RequestData();
    }
}