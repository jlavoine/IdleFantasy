using UnityEngine;
using UnityEngine.SceneManagement;
using MyLibrary;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy {
    public class LoginScreen : MonoBehaviour {
        public GameObject LoginFailurePopup;

        private IdleFantasyBackend mBackend;
        private IMessageService mMessenger;

        private bool mBackendFailure = false;

        private Login mLogin;   // is this the best way...?

        void Start() {
            mMessenger = new MyMessenger();            
            mBackend = new IdleFantasyBackend( mMessenger );
            BackendManager.Init( mBackend );

            mMessenger.AddListener( BackendMessages.LOGIN_SUCCESS, OnLoginSuccess );
            mMessenger.AddListener<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, OnBackendFailure );

            mLogin = new Login( mMessenger, mBackend );
            mLogin.Start();
        }

        void OnDestroy() {
            mLogin.OnDestroy();
            mMessenger.RemoveListener( BackendMessages.LOGIN_SUCCESS, OnLoginSuccess );
            mMessenger.RemoveListener<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, OnBackendFailure );
        }

        private void OnBackendFailure( IBackendFailure i_failure ) {            
            if ( !mBackendFailure ) {
                mBackendFailure = true;
                gameObject.InstantiateUI( LoginFailurePopup );
            }
        }

        private void OnLoginSuccess() {
            StartCoroutine( LoadDataFromBackend() );
        }

        private IEnumerator LoadDataFromBackend() {
            StringTableManager.Init( "English", mBackend, mMessenger );
            Constants.Init( mBackend, mMessenger );
            GenericDataLoader.Init( mBackend, mMessenger );
            GenericDataLoader.LoadDataOfClass<BuildingData>( GenericDataLoader.BUILDINGS );
            GenericDataLoader.LoadDataOfClass<UnitData>( GenericDataLoader.UNITS );

            PlayerData playerData = new PlayerData();
            playerData.Init( mBackend );
            PlayerManager.Init( playerData );

            while ( mBackend.IsBusy() ) {
                yield return 0;
            }

            playerData.CreateManagers();

            DoneLoadingData();
        }

        private void DoneLoadingData() {
            if ( !mBackendFailure ) {
                SceneManager.LoadScene( "Main" );
            }
        }
    }
}