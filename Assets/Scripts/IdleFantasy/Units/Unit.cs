using System;
using MyLibrary;

namespace IdleFantasy {
    public class Unit : IUnit {
        private UnitData mData;

        private ViewModel mModel;
        public ViewModel GetModel() {
            return mModel;
        }

        Upgradeable mLevel;
        public IUpgradeable Level {
            get { return mLevel; }
        }

        public Unit( UnitData i_data ) {
            mModel = new ViewModel();
            mData = i_data;

            mLevel = new Upgradeable();
            mLevel.SetPropertyToUpgrade( mModel, mData.LevelUpgrade );
            Level.Value = 1;
        }

        public string GetID() {
            return mData.ID;
        }

        public float GetProgressFromTimeElapsed( TimeSpan i_timeSpan ) {
            float progressPerSecond = mData.BaseProgressPerSecond / Level.Value;
            float progress = (float)(i_timeSpan.TotalSeconds * progressPerSecond);
            return progress;
        }
    }

}