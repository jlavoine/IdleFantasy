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
            }
        }

        public float NextUnitProgress {
            get { return mModel.GetPropertyValue<float>( "NextUnitProgress" );  }
            set { mModel.SetProperty( "NextUnitProgress", value ); }
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

            UnitData unitData = GenericDataLoader.GetData<UnitData>( Data.Unit );
            Unit = new Unit( unitData, i_unitProgress, mModel );

            UpdateCapacity();
        }

        #region Capacity
        private void OnUpgraded() {
            UpdateCapacity();
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
            NumUnits = 0;
            NextUnitProgress = 0;
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