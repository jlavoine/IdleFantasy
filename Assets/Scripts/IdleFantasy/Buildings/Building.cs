using System;
using System.Collections.Generic;
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
            set { mUnit = value; }
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

        private Upgradeable mBuildingLevel;
        public Upgradeable BuildingLevel {
            get { return mBuildingLevel; }
        }

        public Building( BuildingData i_data, IUnit i_unit ) {
            mModel = new ViewModel();
            mData = i_data;
            Name = i_data.GetName();
            NumUnits = 0;            
            NextUnitProgress = 0;

            Unit = i_unit;

            mBuildingLevel = new Upgradeable();
            mBuildingLevel.SetPropertyToUpgrade( mModel, mData.Level );
            mBuildingLevel.UpgradeEvent += OnUpgraded;
            BuildingLevel.Level = 1;

            UpdateCapacity();
        }

        private void OnUpgraded() {
            UpdateCapacity();
        }

        public void SetUnit( IUnit i_unit ) {
            Unit = i_unit; 
        }

        public void UpdateCapacity() {
            Capacity = Data.Size * BuildingLevel.Level;
        }

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
        #endregion
    }
}