using UnityEngine;

namespace IdleFantasy {
    public class BuildingController : MonoBehaviour {
        private Building mBuilding;

        public void Init( Building i_building ) {
            mBuilding = i_building;
        }

        public void UpgradeClicked() {
            mBuilding.InitiateUpgrade( new FullInventory() );
        }
    }
}