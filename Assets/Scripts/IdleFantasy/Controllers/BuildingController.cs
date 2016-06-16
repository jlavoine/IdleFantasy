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
            mBuilding.Level.InitiateUpgradeWithResources( mInventory );

            BackendManager.Backend.MakeUpgradeCall( GenericDataLoader.BUILDINGS, mBuilding.Data.ID, mBuilding.Level.UpgradeData.PropertyName );
        }

        public void UpgradeUnitClicked() {
            mBuilding.Unit.Level.InitiateUpgradeWithResources( mInventory );

            BackendManager.Backend.MakeUpgradeCall( GenericDataLoader.UNITS, mBuilding.Unit.GetID(), mBuilding.Unit.Level.UpgradeData.PropertyName );
        }
    }
}