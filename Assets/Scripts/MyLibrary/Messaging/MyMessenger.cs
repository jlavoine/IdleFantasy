
namespace MyLibrary {
    public class MyMessenger : IMessageService {
        public void AddListener( string i_event, Callback i_handler ) {
            Messenger.AddListener( i_event, i_handler );
        }

        public void AddListener<T>( string i_event, Callback<T> i_handler ) {
            Messenger.AddListener<T>( i_event, i_handler );
        }

        public void AddListener<T1, T2>( string i_event, Callback<T1, T2> i_handler ) {
            Messenger.AddListener<T1, T2>( i_event, i_handler );
        }

        public void RemoveListener( string i_event, Callback i_handler ) {
            Messenger.RemoveListener( i_event, i_handler );
        }

        public void RemoveListener<T>( string i_event, Callback<T> i_handler ) {
            Messenger.RemoveListener<T>( i_event, i_handler );
        }

        public void RemoveListener<T1, T2>( string i_event, Callback<T1, T2> i_handler ) {
            Messenger.RemoveListener<T1, T2>( i_event, i_handler );
        }

        public void Send( string message ) {
            Messenger.Broadcast( message );
        }

        public void Send<T>( string i_message, T i_param1 ) {
            Messenger.Broadcast<T>( i_message, i_param1 );
        }

        public void Send<T1, T2>( string i_message, T1 i_param1, T2 i_param2 ) {
            Messenger.Broadcast<T1, T2>( i_message, i_param1, i_param2 );
        }
    }
}