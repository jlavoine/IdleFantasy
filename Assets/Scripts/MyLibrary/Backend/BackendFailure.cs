
namespace MyLibrary {
    public class BackendFailure : IBackendFailure {
        private string mMessage;

        public BackendFailure( string i_message ) {
            mMessage = i_message;
        }

        public string GetMessage() {
            return mMessage;
        }
    }
}