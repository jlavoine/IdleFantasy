﻿using UnityEngine;
using UnityEngine.SceneManagement;
using MyLibrary;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy {
    public class LoginScreen : MonoBehaviour {

        private IdleFantasyBackend mBackend;
        private IMessageService mMessenger;

        void Start() {
            mMessenger = new MyMessenger();            
            mBackend = new IdleFantasyBackend( mMessenger );
            BackendManager.Init( mBackend );

            mMessenger.AddListener( BackendMessages.LOGIN_SUCCESS, OnLoginSuccess );

            Login login = new Login( mMessenger, mBackend );
            login.Start();
        }

        void OnDestroy() {
            mMessenger.RemoveListener( BackendMessages.LOGIN_SUCCESS, OnLoginSuccess );
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
            // load next scene here???
           SceneManager.LoadScene( "Main" );
        }
    }
}