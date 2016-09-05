using UnityEngine;
using System.Collections.Generic;
using System;

namespace IdleFantasy {
    public class BuildingMenu : MonoBehaviour {
        public GameObject Content;
        public GameObject BuildingViewPrefab;

        void Start() {
            PopulateMenu();            
        }

        private void PopulateMenu() {
            foreach( Building building in PlayerManager.Data.Buildings ) {
                if ( building.Level.Value > 0 ) {
                    CreateAndInitView( building );
                }
            }
        }

        private void CreateAndInitView( Building i_building ) {
            GameObject buildingViewObject = gameObject.InstantiateUI( BuildingViewPrefab, Content );
            BuildingView buildingView = buildingViewObject.GetComponent<BuildingView>();
            buildingView.Init( i_building );
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