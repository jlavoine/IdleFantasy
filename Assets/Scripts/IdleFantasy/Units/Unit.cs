using System;
using MyLibrary;

namespace IdleFantasy {
    public class Unit : IUnit {
        private UnitData mData;

        private ViewModel mModel;
        public ViewModel GetModel() {
            return mModel;
        }

        private int mLevel;
        public int Level {
            get { return mModel.GetPropertyValue<int>( "Level" );  }
            set { mModel.SetProperty( "Level", value ); }
        }

        public Unit( UnitData i_data ) {
            mModel = new ViewModel();
            mData = i_data;
            Level = 1;
        }

        public string GetID() {
            return mData.ID;
        }

        public float GetProgressFromTimeElapsed( TimeSpan i_timeSpan ) {
            float progressPerSecond = mData.BaseProgressPerSecond / Level;
            float progress = (float)(i_timeSpan.TotalSeconds * progressPerSecond);
            return progress;
        }
    }

}