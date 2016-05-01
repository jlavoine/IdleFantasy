using UnityEngine;

#pragma warning disable 0219

namespace MyLibrary {
    public class LogManager : MonoBehaviour {
        
        void Awake() {
            DontDestroyOnLoad( this );

            IMessageService messenger = new MyMessenger();
            MyLogger logger = new MyLogger( messenger );
        }
    }
}