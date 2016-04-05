
namespace IdleFantasy {
    public class EmptyInventory : IResourceInventory {
        int IResourceInventory.GetResourceCount( string i_resource ) {
            return 0;
        }

        bool IResourceInventory.HasEnoughResources( string i_resource, int i_count ) {
            return false;
        }
    }
}