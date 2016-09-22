
using System;

namespace IdleFantasy {
    public class EmptyInventory : IResourceInventory {
        public void GainResources( string i_resource, int i_value ) {
            throw new NotImplementedException();
        }

        public int GetResourceCount( string i_resource ) {
            return 0;
        }

        public bool HasEnoughResources( string i_resource, int i_count ) {
            return false;
        }

        public void SpendResources( string i_resource, int i_count ) { }
    }
}