using UnityEngine;
using System.Collections.Generic;

namespace IdleFantasy {
    public class BuildingMenu : MonoBehaviour {

        public GameObject BuildingViewPrefab;

        void Start() {
            Dictionary<string, BuildingProgress> buildingProgress = (Dictionary<string,BuildingProgress>)PlayerManager.Data.GetData( PlayerData.BUILDING_PROGRESS );

            foreach ( KeyValuePair<string, BuildingProgress> pair in buildingProgress ) {
                GameObject buildingViewObject = gameObject.InstantiateUI( BuildingViewPrefab );
                BuildingView buildingView = buildingViewObject.GetComponent<BuildingView>();

                BuildingData buildingData = GenericDataLoader.GetData<BuildingData>( GenericDataLoader.BUILDINGS, pair.Key );
                UnitData unitData = GenericDataLoader.GetData<UnitData>( GenericDataLoader.UNITS, buildingData.Unit );
                Unit testUnit = new Unit( unitData );
                Building building = new Building( buildingData, testUnit );

                buildingView.Init( building );
            }
        }
    }
}