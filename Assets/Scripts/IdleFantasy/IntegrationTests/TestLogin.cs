using UnityEngine;
using MyLibrary;

namespace IdleFantasy.IntegrationTests {
    public class TestLogin : MonoBehaviour {

        void Start() {                      
            IBackend playFabBackend = new IdleFantasyBackend();

            MyMessenger.AddListener( BackendMessages.LOGIN_SUCCESS, OnLogin );

            Login login = new Login( playFabBackend, new EmptyAnalyticsTimer() );
            login.Start();
        }

        void OnDestroy() {
            MyMessenger.RemoveListener( BackendMessages.LOGIN_SUCCESS, OnLogin );
        }

        private void OnLogin() {
            IntegrationTest.Pass();
        }
    }
}