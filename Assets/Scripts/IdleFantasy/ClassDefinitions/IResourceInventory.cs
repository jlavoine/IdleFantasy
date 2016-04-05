
public interface IResourceInventory  {
    int GetResourceCount( string i_resource );
    bool HasEnoughResources( string i_resource, int i_count );
}
