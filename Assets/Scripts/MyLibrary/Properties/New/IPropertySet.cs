
namespace MyLibrary {
    public interface IPropertySet {
        T GetPropertyValue<T>( string i_key );
        void CreateProperty( string i_key );
        Property GetProperty( string i_key );
        void SetProperty( string i_key, object i_value );
        bool HasProperty( string i_key );
    }
}