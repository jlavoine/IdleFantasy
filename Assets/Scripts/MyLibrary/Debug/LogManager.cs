using UnityEngine;

namespace MyLibrary {
    public class LogManager : MonoBehaviour {
        
        void Awake() {
            DontDestroyOnLoad( this );

            IMessageService messenger = new MyMessenger();
            MyLogger logger = new MyLogger( messenger );
        }
    }
}