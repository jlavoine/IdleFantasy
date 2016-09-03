using UnityEngine;

namespace MyLibrary {
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        protected static T m_instance;

        public static T Instance {
            get {
                if ( m_instance == null ) {
                    m_instance = (T) FindObjectOfType( typeof( T ) );
                }

                if ( m_instance == null ) {
                    MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Fatal, "Missing singleton for " + typeof( T ), "" );
                }

                return m_instance;
            }
        }

        protected virtual void OnDestroy() {
            m_instance = null;
        }
    }
}