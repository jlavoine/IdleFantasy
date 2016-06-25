using System;
using System.Collections.Generic;
using MyLibrary;

namespace IdleFantasy {
    public delegate void UpgradeComplete();

    public class Upgradeable : IUpgradeable {
        protected ViewModel mModel;
        protected UpgradeData mData;
        
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
                    MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Warn, "Attempt to set " + mData.PropertyName + " above max level.", "Upgradeable" );
                }
                else if ( value < 1 ) {
                    value = 1;
                    MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Warn, "Attempt to set " + mData.PropertyName + " below min level.", "Upgradeable" );
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

                Upgrade();
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
    }
}