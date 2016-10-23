using System;
using MyLibrary;

namespace IdleFantasy {
    public class Building {
        private ViewModel mModel;
        public ViewModel GetViewModel() {
            return mModel;
        }

        private BuildingData mData;
        public BuildingData Data {
            get { return mData; }
        }

        public int NumUnits {
            get { return mModel.GetPropertyValue<int>( "NumUnits" ); }
            set { mModel.SetProperty( "NumUnits", value ); }
        }

        private IUnit mUnit;
        public IUnit Unit {
            get { return mUnit; }
            set {
                if ( mUnit != null ) {
                    mUnit.Level.UpgradeCompleteEvent -= OnUnitUpgraded;
                }

                mUnit = value;
                mUnit.Level.UpgradeCompleteEvent += OnUnitUpgraded;

                UnitID = mUnit.GetID();
            }
        }

        public string UnitID {
            get { return mModel.GetPropertyValue<string>( "UnitID" ); }
            set { mModel.SetProperty( "UnitID", value ); }
        }

        public float NextUnitProgress {
            get { return mModel.GetPropertyValue<float>( "NextUnitProgress" );  }
            set { mModel.SetProperty( "NextUnitProgress", value ); }
        }

        public int NextUnitUpgradeCost {
            get { return mModel.GetPropertyValue<int>( "NextUnitUpgradeCost" ); }
            set { mModel.SetProperty( "NextUnitUpgradeCost", value ); }
        }

        public int NextBuildingUpgradeCost {
            get { return mModel.GetPropertyValue<int>( "NextBuildingUpgradeCost" ); }
            set { mModel.SetProperty( "NextBuildingUpgradeCost", value ); }
        }

        public bool CanUpgradeUnit {            
            get { return mModel.GetPropertyValue<bool>( "CanUpgradeUnit" ); }
            set { mModel.SetProperty( "CanUpgradeUnit", value ); }
        }

        public bool CanUpgradeBuilding {
            get { return mModel.GetPropertyValue<bool>( "CanUpgradeBuilding" ); }
            set { mModel.SetProperty( "CanUpgradeBuilding", value ); }
        }

        public int Capacity {
            get { return mModel.GetPropertyValue<int>( "Capacity" ); }
            set { mModel.SetProperty( "Capacity", value ); }
        }

        public string Name {
            get { return mModel.GetPropertyValue<string>( "Name" ); }
            set { mModel.SetProperty( "Name", value ); }
        }

        private Upgradeable mLevel;
        public Upgradeable Level {
            get { return mLevel; }
        }

        public Building( BuildingProgress i_buildingProgress, UnitProgress i_unitProgress ) {
            mModel = new ViewModel();
            mData = GenericDataLoader.GetData<BuildingData>( i_buildingProgress.ID );
            Name = mData.GetName();
            NumUnits = (int)Math.Floor( i_unitProgress.Count ); 
            NextUnitProgress = i_unitProgress.Count - NumUnits;

            mLevel = new Upgradeable();
            mLevel.SetPropertyToUpgrade( mModel, mData.BuildingLevel );
            mLevel.UpgradeCompleteEvent += OnUpgraded;
            Level.Value = i_buildingProgress.Level;

            UnitData unitData = GenericDataLoader.GetData<UnitData>( i_unitProgress.ID );
            Unit = new Unit( unitData, i_unitProgress, mModel );
            
            UpdateViewProperties();

            SubscribeToMessages();
        }

        public void Dispose() {
            UnsubscribeFromMessages();
        }

        private void SubscribeToMessages() {
            EasyMessenger.Instance.AddListener<string, int>( PlayerData.INVENTORY_CHANGED_EVENT, OnInventoryChanged );
            EasyMessenger.Instance.AddListener( MapKeys.WORLD_RESET_SUCCESS, OnWorldReset );
        }

        private void UnsubscribeFromMessages() {
            EasyMessenger.Instance.RemoveListener<string, int>( PlayerData.INVENTORY_CHANGED_EVENT, OnInventoryChanged );
            EasyMessenger.Instance.RemoveListener( MapKeys.WORLD_RESET_SUCCESS, OnWorldReset );
        }

        private void OnInventoryChanged( string i_resource, int i_newValue ) {
            UpdateViewProperties();
        }

        private void OnWorldReset() {
            Level.Value = 1;
            Unit.Level.Value = 1;
            Unit.TrainingLevel = 0;
            NumUnits = 0;
            NextUnitProgress = 0f;

            UpdateViewProperties();
        }

        private void UpdateViewProperties() {
            UpdateCapacity();
            UpdateUpgradeCostProperties();
            UpdateAllowedUpgradeProperties();
        }

        private void UpdateUpgradeCostProperties() {
            NextUnitUpgradeCost = Unit.Level.GetUpgradeCostForResource( VirtualCurrencies.GOLD );
            NextBuildingUpgradeCost = Level.GetUpgradeCostForResource( VirtualCurrencies.GOLD );
        }

        private void UpdateAllowedUpgradeProperties() {
            // FIXME: Did this because tests were failing...need a better way
            if ( PlayerManager.Data is IResourceInventory ) {
                CanUpgradeUnit = Unit.Level.CanAffordUpgrade( (IResourceInventory) PlayerManager.Data ) && !Unit.Level.IsAtMaxLevel();
                CanUpgradeBuilding = Level.CanAffordUpgrade( (IResourceInventory) PlayerManager.Data ) && !Level.IsAtMaxLevel();
            }
        }

        #region Capacity
        private void OnUpgraded() {            
            UpdateViewProperties();
        }    

        public void UpdateCapacity() {
            Capacity = Data.StartingSize * Level.Value;
        }
        #endregion

        #region Unit Generation
        public void Tick( TimeSpan i_timeSpan ) {
            if ( AtMaxCapacity() ) {
                return;
            }

            float progress = Unit.GetProgressFromTimeElapsed( i_timeSpan );

            NextUnitProgress += progress;

            if ( NextUnitProgress > 1 ) {
                int numNewUnits = (int)Math.Floor( NextUnitProgress );
                AddUnitsFromProgress( numNewUnits );
                NextUnitProgress -= numNewUnits;
            }

            if ( AtMaxCapacity() ) {
                NextUnitProgress = 0;
            }
        }

        public bool AtMaxCapacity() {
            return NumUnits == Capacity;
        }

        public void AddUnitsFromProgress( int i_numUnits ) {
            if ( i_numUnits < 1 ) {
                return;
            }

            NumUnits = Math.Min( i_numUnits + NumUnits, Capacity );
        }

        public void OnUnitUpgraded() {
            UpdateViewProperties();
        }
        #endregion

        #region Unit Power
        public long GetStatTotal( string i_stat ) {
            int statPerUnit = Unit.GetBaseStat( i_stat );
            int total = NumUnits * statPerUnit;

            return total;
        }
        #endregion
    }
}