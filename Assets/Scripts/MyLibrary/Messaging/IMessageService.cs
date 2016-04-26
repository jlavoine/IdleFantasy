
namespace MyLibrary {
    public interface IMessageService {
        void Send( string i_event );
        void Send<T>( string i_event, T param1 );
        void Send<T1, T2>( string i_event, T1 i_param1, T2 i_param2 );
        void Send<T1, T2, T3>( string i_event, T1 i_param1, T2 i_param2, T3 i_param3 );

        void AddListener( string i_event, Callback i_handler );
        void AddListener<T>( string i_event, Callback<T> i_handler );
        void AddListener<T1, T2>( string i_event, Callback<T1, T2> i_handler );
        void AddListener<T1, T2, T3>( string i_event, Callback<T1, T2, T3> i_handler );

        void RemoveListener( string i_event, Callback i_handler );
        void RemoveListener<T>( string i_event, Callback<T> i_handler );
        void RemoveListener<T1, T2>( string i_event, Callback<T1, T2> i_handler );
        void RemoveListener<T1, T2, T3>( string i_event, Callback<T1, T2, T3> i_handler );
    }
}