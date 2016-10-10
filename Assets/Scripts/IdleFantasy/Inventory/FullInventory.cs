
using System;

namespace IdleFantasy {
    public class FullInventory : IResourceInventory {
        int IResourceInventory.GetResourceCount( string i_resource ) {
            return int.MaxValue;
        }

        bool IResourceInventory.HasEnoughResources( string i_resource, int i_count ) {
            return true;
        }

        public void SpendResources( string i_resource, int i_count ) { }

        public void GainResources( string i_resource, int i_value ) {
            throw new NotImplementedException();
        }

        public void SetResources( string i_resource, int i_amount ) {
            throw new NotImplementedException();
        }
    }
}