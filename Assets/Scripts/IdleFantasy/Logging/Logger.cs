using UnityEngine;
using System.Collections;

namespace IdleFantasy {
    public class Logger {
        public static void Log(string i_message, LogTypes i_logType) {
            switch (i_logType) {
                case LogTypes.Error:
                    UnityEngine.Debug.LogError( i_message );
                    break;
                default:
                    UnityEngine.Debug.Log( i_message );
                    break;
            }
        }
    }
}