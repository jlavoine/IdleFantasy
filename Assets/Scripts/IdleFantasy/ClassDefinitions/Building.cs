using System.Collections.Generic;

namespace IdleFantasy {
    public class Building {
        private BuildingData mData;
        public BuildingData Data {
            get {
                return mData;
            }
        }

        private int mLevel;
        public int Level {
            get {
                return mLevel;
            }
            set {
                mLevel = value;
            }
        }

        public Building( BuildingData i_data ) {
            mData = i_data;
        }

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
    }
}