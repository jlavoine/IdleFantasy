
namespace MyLibrary {
    public class EmptyLogger : ILogService {
        public void Log( LogTypes i_type, string i_message, string i_category = null ) {}
    }
}