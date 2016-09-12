
namespace MyLibrary {
    public class EasyLogger : ILogService {
        private static ILogService mInstance;
        public static ILogService Instance {
            get {
                if ( mInstance == null ) {
                    mInstance = new EasyLogger();
                }

                return mInstance;
            }
            set {
                // unit tests only!
                mInstance = value;
            }
        }

        public void Log( LogTypes i_type, string i_message, string i_category = null ) {
            i_message = string.IsNullOrEmpty( i_category ) ? i_message : i_category + ": " + i_message;

            // TODO: make this better
            switch ( i_type ) {
                case LogTypes.Info:
                    UnityEngine.Debug.Log( i_message );
                    break;
                case LogTypes.Error:
                case LogTypes.Fatal:
                case LogTypes.Warn:
                    UnityEngine.Debug.LogError( i_message );
                    break;
                default:
                    UnityEngine.Debug.LogError( "Debug type " + i_type + " unsupported!" );
                    break;
            }
        }
    }
}