using UnityEngine;
using System.Collections.Generic;

namespace IdleFantasy {
    public class BuildingMenu : MonoBehaviour {

        public GameObject BuildingViewPrefab;

        void Start() {
            Dictionary<string, int> buildingLevels = (Dictionary<string,int>)PlayerManager.Data.GetData( "BuildingLevels" );

            foreach ( KeyValuePair<string, int> pair in buildingLevels ) {
                GameObject view = Instantiate( BuildingViewPrefab );
                view.transform.SetParent( this.transform );
            }
        }
    }
}