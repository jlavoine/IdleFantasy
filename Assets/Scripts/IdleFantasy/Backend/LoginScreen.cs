using UnityEngine;
using MyLibrary;

namespace IdleFantasy {
    public class LoginScreen : MonoBehaviour {

        void Start() {
            IMessageService myMessenger = new MyMessenger();            

            IBackend playFabBackend = new PlayFabBackend( myMessenger );

            Login login = new Login( myMessenger, playFabBackend );
            login.Start();
        }

        void OnDestroy() {

        }
    }
}