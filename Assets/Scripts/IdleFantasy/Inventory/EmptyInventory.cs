
namespace IdleFantasy {
    public class EmptyInventory : IResourceInventory {
        public int GetResourceCount( string i_resource ) {
            return 0;
        }

        public bool HasEnoughResources( string i_resource, int i_count ) {
            return false;
        }

        public void SpendResources( string i_resource, int i_count ) { }
    }
}