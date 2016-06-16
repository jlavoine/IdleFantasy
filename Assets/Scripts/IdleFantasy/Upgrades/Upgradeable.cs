using System;
using System.Collections.Generic;
using MyLibrary;

namespace IdleFantasy {
    public delegate void UpgradeComplete();

    public class Upgradeable : IUpgradeable {
        private ViewModel mModel;
        private UpgradeData mData;
        
        public event UpgradeComplete UpgradeCompleteEvent;

        public void SetPropertyToUpgrade( ViewModel i_model, UpgradeData i_data ) {
            mModel = i_model;
            mData = i_data;
        }

        public UpgradeData UpgradeData {
            get { return mData; }
        }

        public int Value {
            get { return mModel.GetPropertyValue<int>( mData.PropertyName ); }
            set {
                if ( value > mData.MaxLevel ) {
                    value = mData.MaxLevel;
                    Messenger.Broadcast<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Warn, "Attempt to set " + mData.PropertyName + " above max level.", "Upgradeable" );
                }
                else if ( value < 1 ) {
                    value = 1;
                    Messenger.Broadcast<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Warn, "Attempt to set " + mData.PropertyName + " below min level.", "Upgradeable" );
                }

                mModel.SetProperty( mData.PropertyName, value );
            }
        }

        public int MaxLevel {
            get { return mData.MaxLevel; }
        }

        public Dictionary<string, int> ResourcesToUpgrade {
            get { return mData.ResourcesToUpgrade; }
        }

        public void InitiateUpgradeWithResources( IResourceInventory i_inventory ) {
            if ( CanUpgrade( i_inventory ) ) {
                ChargeForUpgrade( i_inventory );

                AddProgress( 1f );
            }
        }

        public void ChargeForUpgrade( IResourceInventory i_inventory ) {
            foreach ( KeyValuePair<string, int> cost in mData.ResourcesToUpgrade ) {
                string resourceName = cost.Key;
                int resourceAmount = GetUpgradeCostForResource( resourceName );
                i_inventory.SpendResources( resourceName, resourceAmount );
            }
        }

        public void Upgrade() {
            Value++;

            if ( UpgradeCompleteEvent != null ) {
                UpgradeCompleteEvent();
            }
        }

        public bool CanAffordUpgrade( IResourceInventory i_inventory ) {
            foreach ( KeyValuePair<string, int> cost in mData.ResourcesToUpgrade ) {
                int resourceCost = GetUpgradeCostForResource( cost.Key );
                if ( i_inventory.HasEnoughResources( cost.Key, resourceCost ) == false ) {
                    return false;
                }
            }
            return true;
        }

        public bool CanUpgrade( IResourceInventory i_inventory ) {
            if ( IsAtMaxLevel() ) {
                return false;
            }

            return CanAffordUpgrade( i_inventory );
        }

        public bool IsAtMaxLevel() {
            bool atMaxLevel = Value == mData.MaxLevel;
            return atMaxLevel;
        }

        public int GetUpgradeCostForResource( string i_resource ) {            
            if ( mData.ResourcesToUpgrade.ContainsKey( i_resource ) ) {
                int cost = mData.ResourcesToUpgrade[i_resource] * Value;
                return cost;
            }
            else {
                return int.MaxValue;
            }
        }

        #region Points
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
                int xpToAdd = (int)( progressToAdd * GetTotalPointsForNextLevel() );
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
        #endregion
    }
}