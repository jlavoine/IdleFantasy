
namespace MyLibrary {
    public interface ILogService {
        void Log( LogTypes i_type, string i_message, string i_category = null );
    }
}