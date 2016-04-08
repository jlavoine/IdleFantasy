
namespace IdleFantasy {
    public class FullInventory : IResourceInventory {
        int IResourceInventory.GetResourceCount( string i_resource ) {
            return int.MaxValue;
        }

        bool IResourceInventory.HasEnoughResources( string i_resource, int i_count ) {
            return true;
        }

        public void SpendResources( string i_resource, int i_count ) { }
    }
}