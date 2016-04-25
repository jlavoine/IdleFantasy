using UnityEngine;
using MyLibrary;

namespace IdleFantasy.IntegrationTests {
    public class TestLogin : MonoBehaviour {
        private IMessageService mMessenger;

        void Start() {
            mMessenger = new MyMessenger();                       
            ILogService myLogger = new MyLogger();
            IBackend playFabBackend = new PlayFabBackend( mMessenger, myLogger );

            mMessenger.AddListener( BackendMessages.LOGIN_SUCCESS, OnLogin );

            Login login = new Login( mMessenger, myLogger, playFabBackend );
            login.Start();
        }

        void OnDestroy() {
            mMessenger.RemoveListener( BackendMessages.LOGIN_SUCCESS, OnLogin );
        }

        private void OnLogin() {
            IntegrationTest.Pass();
        }
    }
}