using UnityEngine;
using System.Collections.Generic;
using System;

namespace IdleFantasy {
    public class NormalInventory : IResourceInventory {
        private Dictionary<string, int> mInventory = new Dictionary<string, int>();

        public void SetResource( string i_resource, int i_amount ) {
            if (!mInventory.ContainsKey(i_resource)) {
                mInventory[i_resource] = 0;
            }

            mInventory[i_resource] = i_amount;
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

            //UpdateInventoryData();
        }
    }
}
