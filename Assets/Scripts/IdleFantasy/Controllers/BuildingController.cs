using UnityEngine;

namespace IdleFantasy {
    public class BuildingController : MonoBehaviour {
        private Building mBuilding;
        private IResourceInventory mInventory;

        public void Init( Building i_building, IResourceInventory i_inventory ) {
            mBuilding = i_building;
            mInventory = i_inventory;
        }

        public void UpgradeClicked() {
            mBuilding.BuildingLevel.InitiateUpgrade( mInventory );
        }
    }
}