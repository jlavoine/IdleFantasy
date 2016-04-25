using UnityEngine;
using MyLibrary;

namespace IdleFantasy {
    public class LoginScreen : MonoBehaviour {

        void Start() {
            IMessageService myMessenger = new MyMessenger();            
            ILogService myLogger = new MyLogger();

            IBackend playFabBackend = new PlayFabBackend( myMessenger, myLogger );

            Login login = new Login( myMessenger, myLogger, playFabBackend );
            login.Start();
        }

        void OnDestroy() {

        }
    }
}