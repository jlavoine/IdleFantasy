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

        private bool mBackendFailure = false;

        private Login mLogin;   // is this the best way...?

        void Start() {        
            mBackend = new IdleFantasyBackend();
            BackendManager.Init( mBackend );

            MyMessenger.AddListener( BackendMessages.LOGIN_SUCCESS, OnLoginSuccess );
            MyMessenger.AddListener<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, OnBackendFailure );

            mLogin = new Login( mBackend );
            mLogin.Start();
        }

        void OnDestroy() {
            mLogin.OnDestroy();
            MyMessenger.RemoveListener( BackendMessages.LOGIN_SUCCESS, OnLoginSuccess );
            MyMessenger.RemoveListener<IBackendFailure>( BackendMessages.BACKEND_REQUEST_FAIL, OnBackendFailure );
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
            StringTableManager.Init( "English", mBackend );
            Constants.Init( mBackend );
            GenericDataLoader.Init( mBackend );
            GenericDataLoader.LoadDataOfClass<BuildingData>( GenericDataLoader.BUILDINGS );
            GenericDataLoader.LoadDataOfClass<UnitData>( GenericDataLoader.UNITS );
            GenericDataLoader.LoadDataOfClass<GuildData>( GenericDataLoader.GUILDS );

            while ( mBackend.IsBusy() ) {
                yield return 0;
            }

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