using System;
using System.Collections.Generic;

namespace IdleFantasy {
    public class Building {
        private BuildingData mData;
        public BuildingData Data {
            get { return mData; }
        }

        private int mLevel;
        public int Level {
            get { return mLevel; }
            set { mLevel = value; }
        }

        private int mNumUnits;
        public int NumUnits {
            get { return mNumUnits; }
            set { mNumUnits = value; }
        }

        private IUnit mUnit;
        public IUnit Unit {
            get { return mUnit; }
            set { mUnit = value; }
        }

        private float mNextUnitProgress;
        public float NextUnitProgress {
            get { return mNextUnitProgress;  }
            set { mNextUnitProgress = value; }
        }

        public Building( BuildingData i_data ) {
            mData = i_data;
        }

        public void SetUnit( IUnit i_unit ) {
            Unit = i_unit; 
        }

        public int GetMaxUnits() {
            return Data.Size * Level;
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
            float progress = Unit.GetProgressFromTimeElapsed( i_timeSpan );

            NextUnitProgress += progress;

            if ( NextUnitProgress > 1 ) {
                int numNewUnits = (int)Math.Floor( NextUnitProgress );
                AddUnitsFromProgress( numNewUnits );
                NextUnitProgress -= numNewUnits;
            }
        }

        public void AddUnitsFromProgress( int i_numUnits ) {
            if ( i_numUnits < 1 ) {
                return;
            }

            NumUnits = Math.Min( i_numUnits + NumUnits, GetMaxUnits() );
        }
        #endregion
    }
}