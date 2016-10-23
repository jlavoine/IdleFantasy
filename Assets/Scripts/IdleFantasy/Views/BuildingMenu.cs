using UnityEngine;
using System;
using MyLibrary;

namespace IdleFantasy {
    public class BuildingMenu : MonoBehaviour {
        public GameObject Content;
        public GameObject BuildingViewPrefab;

        void Start() {
            PopulateMenu();

            SubscribeToMessages();      
        }

        void OnDestroy() {
            UnsubscribeFromMessages();
        }

        private void SubscribeToMessages() {
            MyMessenger.AddListener<string>( UnitKeys.UNLOCK_EVENT, OnUnitUnlocked );
        }

        private void UnsubscribeFromMessages() {
            MyMessenger.RemoveListener<string>( UnitKeys.UNLOCK_EVENT, OnUnitUnlocked );
        }

        private void OnUnitUnlocked( string i_unitID ) {
            RefreshMenu();
        }

        private void RefreshMenu() {
            Content.DestroyChildren();
            PopulateMenu();
        }

        private void PopulateMenu() {
            for ( int i = 0; i < PlayerManager.Data.Buildings.Count; ++i ) {
                Building building = PlayerManager.Data.Buildings[i];
                if ( building.Level.Value > 0 ) {
                    CreateAndInitView( building, i );
                }
            }
        }

        private void CreateAndInitView( Building i_building, int i_index ) {
            GameObject buildingViewObject = gameObject.InstantiateUI( BuildingViewPrefab, Content );
            BuildingView buildingView = buildingViewObject.GetComponent<BuildingView>();
            buildingView.Init( i_building, i_index );
        }

        void Update() {
            int msElapsed = (int) ( Time.deltaTime * 1000 );
            TimeSpan timeElapsedAsSpan = new TimeSpan( 0, 0, 0, 0, msElapsed );

            foreach ( Building building in PlayerManager.Data.Buildings ) {
                building.Tick( timeElapsedAsSpan );
            }            
        }
    }
}