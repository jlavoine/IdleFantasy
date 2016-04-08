using System.Collections.Generic;
using MyLibrary;
using System;

namespace IdleFantasy {
    public class MockPlayerData : IPlayerData, IResourceInventory {
        private ViewModel mModel;

        private Dictionary<string, int> mInventory = new Dictionary<string, int>();

        public MockPlayerData() {
            mModel = new ViewModel();
        }

        public int Gold {
            get { return mModel.GetPropertyValue<int>( "Gold" );  }
            set {
                mModel.SetProperty( "Gold", value );

                if ( !mInventory.ContainsKey( "Gold" ) ) {
                    mInventory.Add( "Gold", value );
                }
                else {
                    mInventory["Gold"] = value;
                }
            }
        }

        public ViewModel GetViewModel() {
            return mModel;
        }

        public int GetResourceCount( string i_resource ) {
            if ( mInventory.ContainsKey( i_resource ) ) {
                return mInventory[i_resource];                   
            }
            else {
                mInventory[i_resource] = 0;
                return 0;
            }
        }

        public bool HasEnoughResources( string i_resource, int i_count ) {
            int amountOfResource = GetResourceCount( i_resource );
            bool hasEnough = amountOfResource >= i_count;
            return hasEnough;
        }

        public void SpendResources( string i_resource, int i_count ) {
            int amountOfResource = GetResourceCount( i_resource );
            int remainingValue = Math.Max( amountOfResource - i_count, 0 );
            mInventory[i_resource] = remainingValue;

            UpdateInventoryData();
        }

        public void UpdateInventoryData() {
            foreach ( KeyValuePair<string, int> inventoryItem in mInventory ) {
                mModel.SetProperty( inventoryItem.Key, inventoryItem.Value );
            }
        }
    }
}