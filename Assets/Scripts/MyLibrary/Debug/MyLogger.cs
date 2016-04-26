using UnityEngine;

namespace MyLibrary {
    public class MyLogger {
        public const string LOG_EVENT = "Log";

        private IMessageService mMessenger;

        public MyLogger( IMessageService i_messenger ) {
            mMessenger = i_messenger;

            mMessenger.AddListener<LogTypes, string, string>( LOG_EVENT, LogWithCategory );
        }

        public void Dispose() {
            mMessenger.RemoveListener<LogTypes, string, string>( LOG_EVENT, LogWithCategory );
        }

        public void LogWithCategory( LogTypes i_type, string i_message, string i_category ) {
            i_message = string.IsNullOrEmpty( i_category ) ? i_message : i_category + ": " + i_message;

            // TODO: make this better
            switch ( i_type ) {
                case LogTypes.Info:
                    UnityEngine.Debug.Log( i_message );
                    break;
                case LogTypes.Error:
                    UnityEngine.Debug.LogError( i_message );
                    break;
                case LogTypes.Warn:
                    UnityEngine.Debug.Log( "WARNING! " + i_message );
                    break;
                default:
                    UnityEngine.Debug.LogError( "Debug type " + i_type + " unsupported!" );
                    break;
            }
        }
    }
}