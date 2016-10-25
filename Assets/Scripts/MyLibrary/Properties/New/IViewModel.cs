
namespace MyLibrary {
    public interface IViewModel {
        bool HasProperty( string i_propertyName );

        void CreateProperty( string i_propertyName );

        Property GetProperty( string i_propertyName );

        T GetPropertyValue<T>( string i_propertyName );
    }
}
