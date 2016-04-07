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

        public int Level {
            get { return mModel.GetPropertyValue<int>( "Level" ); }
            set { mModel.SetProperty( "Level", value ); }
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

        public Building( BuildingData i_data, IUnit i_unit ) {
            mModel = new ViewModel();
            mData = i_data;
            Name = i_data.GetName();
            NumUnits = 0;
            Level = 1;
            NextUnitProgress = 0;

            Unit = i_unit;

            UpdateCapacity();
        }

        public void SetUnit( IUnit i_unit ) {
            Unit = i_unit; 
        }

        public void UpdateCapacity() {
            Capacity = Data.Size * Level;
        }

        #region Upgrading
        public void InitiateUpgrade( IResourceInventory i_inventory ) {
            if ( CanUpgrade( i_inventory ) ) {
                Upgrade();
            }
        }

        public bool CanUpgrade( IResourceInventory i_inventory ) {
            foreach(KeyValuePair<string,int> cost in mData.ResourcesToUpgrade) {
                if(i_inventory.HasEnoughResources(cost.Key, cost.Value) == false) {
                    return false;
                }
            }

            return true;
        }

        public void Upgrade() {
            Level++;

            if ( Level > mData.MaxLevel ) {
                Level = mData.MaxLevel;
                //Logger.Log( "Upgrading " + mData.ID + " over max level.", LogTypes.Error );
            } else if ( Level < 1 ) {
                Level = 1;
                //Logger.Log( "Upgrading " + mData.ID + " below min level.", LogTypes.Error );
            }
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
        #endregion
    }
}