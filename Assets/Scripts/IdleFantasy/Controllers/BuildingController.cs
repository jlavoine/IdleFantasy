using UnityEngine;
using MyLibrary;

namespace IdleFantasy {
    public class BuildingController : MonoBehaviour {
        public const string UNIT_UPGRADED_MESSAGE = "UnitUpgraded"; // for tutorial
        public const string BUILDING_UPGRADED_MESSAGE = "BuildingUpgraded"; // for tutorial

        private Building mBuilding;
        private IResourceInventory mInventory;

        public void Init( Building i_building, IResourceInventory i_inventory ) {
            mBuilding = i_building;
            mInventory = i_inventory;
        }

        public void UpgradeClicked() {
            MyMessenger.Send( BUILDING_UPGRADED_MESSAGE );

            mBuilding.Level.InitiateUpgradeWithResources( mInventory );

            BackendManager.Backend.MakeUpgradeCall( GenericDataLoader.BUILDINGS, mBuilding.Data.ID, mBuilding.Level.UpgradeData.PropertyName );
        }

        public void UpgradeUnitClicked() {
            MyMessenger.Send( UNIT_UPGRADED_MESSAGE );

            mBuilding.Unit.Level.InitiateUpgradeWithResources( mInventory );

            BackendManager.Backend.MakeUpgradeCall( GenericDataLoader.UNITS, mBuilding.Unit.GetID(), mBuilding.Unit.Level.UpgradeData.PropertyName );
        }
    }
}