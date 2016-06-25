
namespace MyLibrary {
    public static class MyMessenger {
        public static void AddListener( string i_event, Callback i_handler ) {
            Messenger.AddListener( i_event, i_handler );
        }

        public static void AddListener<T>( string i_event, Callback<T> i_handler ) {
            Messenger.AddListener<T>( i_event, i_handler );
        }

        public static void AddListener<T1, T2>( string i_event, Callback<T1, T2> i_handler ) {
            Messenger.AddListener<T1, T2>( i_event, i_handler );
        }

        public static void AddListener<T1, T2, T3>( string i_event, Callback<T1, T2, T3> i_handler ) {
            Messenger.AddListener<T1, T2, T3>( i_event, i_handler );
        }

        public static void RemoveListener( string i_event, Callback i_handler ) {
            Messenger.RemoveListener( i_event, i_handler );
        }

        public static void RemoveListener<T>( string i_event, Callback<T> i_handler ) {
            Messenger.RemoveListener<T>( i_event, i_handler );
        }

        public static void RemoveListener<T1, T2>( string i_event, Callback<T1, T2> i_handler ) {
            Messenger.RemoveListener<T1, T2>( i_event, i_handler );
        }

        public static void RemoveListener<T1, T2, T3>( string i_event, Callback<T1, T2, T3> i_handler ) {
            Messenger.RemoveListener<T1, T2, T3>( i_event, i_handler );
        }

        public static void Send( string message ) {
            Messenger.Broadcast( message );
        }

        public static void Send<T>( string i_message, T i_param1 ) {
            Messenger.Broadcast<T>( i_message, i_param1 );
        }

        public static void Send<T1, T2>( string i_message, T1 i_param1, T2 i_param2 ) {
            Messenger.Broadcast<T1, T2>( i_message, i_param1, i_param2 );
        }

        public static void Send<T1, T2, T3>( string i_message, T1 i_param1, T2 i_param2, T3 i_param3 ) {
            Messenger.Broadcast<T1, T2, T3>( i_message, i_param1, i_param2, i_param3 );
        }
    }
}