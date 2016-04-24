
namespace MyLibrary {
    public class MyMessenger : IMessageService {
        public void Send( string message ) {
            Messenger.Broadcast( message );
        }

        public void Send<T>( string message, T param1 ) {
            Messenger.Broadcast<T>( message, param1 );
        }
    }
}