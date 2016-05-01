using UnityEngine;
using MyLibrary;
using System.Collections;

namespace IdleFantasy {
    public class LoginScreen : MonoBehaviour {

        private IBackend mBackend;
        private IMessageService mMessenger;

        void Start() {
            mMessenger = new MyMessenger();            
            mBackend = new PlayFabBackend( mMessenger );

            mMessenger.AddListener( BackendMessages.LOGIN_SUCCESS, OnLoginSuccess );

            Login login = new Login( mMessenger, mBackend );
            login.Start();
        }

        void OnDestroy() {
            mMessenger.RemoveListener( BackendMessages.LOGIN_SUCCESS, OnLoginSuccess );
        }

        private void OnLoginSuccess() {
            StartCoroutine( GetTitleAndUserData() );
        }

        private IEnumerator GetTitleAndUserData() {
            StringTableManager.Init( "English", mBackend, mMessenger );
            BuildingLoader.Init( mBackend );

            while ( mBackend.IsBusy() ) {
                yield return 0;
            }

            DoneLoadingData();
        }

        private void DoneLoadingData() {
            // load next scene here???
        }
    }
}