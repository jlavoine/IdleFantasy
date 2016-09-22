
public interface IResourceInventory  {
    int GetResourceCount( string i_resource );
    bool HasEnoughResources( string i_resource, int i_value );
    void SpendResources( string i_resource, int i_value );
    void GainResources( string i_resource, int i_value );
}
