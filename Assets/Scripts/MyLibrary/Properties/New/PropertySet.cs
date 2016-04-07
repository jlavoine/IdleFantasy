
using System.Collections.Generic;

namespace MyLibrary {
    public class PropertySet : IPropertySet {
        public Dictionary<string, Property> Properties = new Dictionary<string, Property>();

        public void CreateProperty( string i_key ) {
            SetProperty( i_key, null );
        }

        public Property GetProperty( string i_key ) {
            if ( HasProperty( i_key ) ) {
                return Properties[i_key];
            }
            else {
                Debug.LogError( "Trying to get a property that doesn't exist: " + i_key );
                return null;
            }
        }

        public T GetPropertyValue<T>( string i_key ) {
            T value = default( T );

            if ( Properties.ContainsKey( i_key ) ) {
                Property property = Properties[i_key];
                value = property.GetValue<T>();
            }

            return value;
        }

        public bool HasProperty( string i_key ) {
            if ( Properties == null ) { 
                Properties = new Dictionary<string, Property>();
            }

            return Properties.ContainsKey( i_key );
        }

        public void SetProperty( string i_key, object i_value ) {
            if ( !HasProperty( i_key ) ) {
                Properties.Add( i_key, new Property( i_key ) );
            }

            Property property = Properties[i_key];
            property.SetValue( i_value );
        }
    }
}