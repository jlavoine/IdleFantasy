
namespace MyLibrary {
    public interface IMessageService {
        void Send( string message );
        void Send<T>( string message, T param1 );
    }
}