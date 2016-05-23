using UnityEngine;
using System.Collections.Generic;
using System;

namespace IdleFantasy {
    public class BuildingMenu : MonoBehaviour {

        public GameObject BuildingViewPrefab;

        private List<Building> mBuildings = new List<Building>();

        void Start() {
            PopulateMenu();            
        }

        private void PopulateMenu() {
            Dictionary<string, BuildingProgress> buildingProgress = (Dictionary<string, BuildingProgress>) PlayerManager.Data.GetData( PlayerData.BUILDING_PROGRESS );

            foreach ( KeyValuePair<string, BuildingProgress> pair in buildingProgress ) {                
                Building building = CreateBuilding( pair.Key, pair.Value );

                CreateAndInitView( building );

                mBuildings.Add( building );
            }
        }

        private void CreateAndInitView( Building i_building ) {
            GameObject buildingViewObject = gameObject.InstantiateUI( BuildingViewPrefab );
            BuildingView buildingView = buildingViewObject.GetComponent<BuildingView>();
            buildingView.Init( i_building );
        }

        private Building CreateBuilding( string i_key, BuildingProgress i_progress ) {
            BuildingData buildingData = GenericDataLoader.GetData<BuildingData>( GenericDataLoader.BUILDINGS, i_key );
            UnitData unitData = GenericDataLoader.GetData<UnitData>( GenericDataLoader.UNITS, buildingData.Unit );
            Unit testUnit = new Unit( unitData );
            BuildingProgress buildingProgress = PlayerManager.Data.BuildingProgress[buildingData.ID];
            Building building = new Building( buildingData, testUnit, buildingProgress );

            return building;
        }

        void Update() {
            int msElapsed = (int) ( Time.deltaTime * 1000 );
            TimeSpan timeElapsedAsSpan = new TimeSpan( 0, 0, 0, 0, msElapsed );

            foreach ( Building building in mBuildings ) {
                building.Tick( timeElapsedAsSpan );
            }            
        }
    }
}