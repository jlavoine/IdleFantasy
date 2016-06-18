using System;
using System.Collections.Generic;
using MyLibrary;

namespace IdleFantasy {
    public class PointsUpgradeable : Upgradeable {
        public const string POINTS = "Points";
        public const string PROGRESS = "Progress";

        public int Points {
            get { return mModel.GetPropertyValue<int>( POINTS ); }
            set {
                if ( value < 0 ) {
                    value = 0;
                    Messenger.Broadcast<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Warn, "Attempt to set " + mData.PropertyName + " points below 0.", "Upgradeable" );
                }

                UpdatePointsProperty( value );
                UpdateProgressValue();
                CheckForUpgrade();
            }
        }

        public float Progress {
            get { return mModel.GetPropertyValue<float>( PROGRESS ); }
            set {
                value = Math.Max( 0, value );
                value = Math.Min( 1, value );
                mModel.SetProperty( PROGRESS, value );
            }
        }

        public void AddProgress( float i_progress ) {
            do {
                float progressToAdd = Math.Min( i_progress, 1 - Progress );
                progressToAdd = Math.Max( 0, progressToAdd );
                int xpToAdd = (int) ( progressToAdd * GetTotalPointsForNextLevel() );
                Points += xpToAdd;
                i_progress -= progressToAdd;
            } while ( i_progress > 0 );
        }

        private void UpdatePointsProperty( int i_value ) {
            mModel.SetProperty( POINTS, i_value );
        }

        private void UpdateProgressValue() {
            int pointsToLevel = GetTotalPointsForNextLevel();
            float progress = Points / pointsToLevel;

            Progress = progress;
        }

        private int GetTotalPointsForNextLevel() {
            return UpgradeData.BaseXpToLevel * Value;
        }

        private void CheckForUpgrade() {
            int pointsToLevel = GetTotalPointsForNextLevel();
            if ( Points >= pointsToLevel ) {
                Upgrade();
                Points = Points - pointsToLevel;
                CheckForUpgrade();
            }
        }
    }
}
