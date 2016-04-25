using UnityEngine;

namespace MyLibrary {
    public class MyLogger : ILogService {

        public void Log( LogTypes i_type, string i_message, string i_category = null ) {
            i_message = string.IsNullOrEmpty( i_category ) ? i_message : i_category + ": " + i_message;

            // TODO: make this better
            switch ( i_type ) {
                case LogTypes.Info:
                    UnityEngine.Debug.Log( i_message );
                    break;
                case LogTypes.Error:
                    UnityEngine.Debug.LogError( i_message );
                    break;
                default:
                    UnityEngine.Debug.LogError( "Debug type " + i_type + " unsupported!" );
                    break;
            }
        }
    }
}