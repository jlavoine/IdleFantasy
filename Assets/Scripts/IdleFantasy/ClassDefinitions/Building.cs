
namespace IdleFantasy {
    public class Building {
        private BuildingData mData;

        private int mLevel;
        public int Level {
            get {
                return mLevel;
            }
            set {
                mLevel = value;
            }
        }

        public Building( BuildingData i_data) {
            mData = i_data;
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