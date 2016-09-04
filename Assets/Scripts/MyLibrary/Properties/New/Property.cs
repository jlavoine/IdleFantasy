using System;

namespace MyLibrary {
    public class Property {
        private Guid mID;
        public Guid ID {
            get { return mID; }
        }

        private string mName;
        public string Name {
            get { return mName;  }
        }

        private object mValue;
        public T GetValue<T>() {
            if ( mValue is T ) {
                return (T) mValue;
            }
            else {
                Debug.Log( LogTypes.Error, "Property " + Name + " is wrong value.", "Property" );
                return default( T );
            }
        }

        public void SetValue( object i_object ) {
            mValue = i_object;

            Messenger.Broadcast( "SetDirty_" + ID );
        }

        public Property( string i_name, object i_value = null ) {
            mID = Guid.NewGuid();
            mName = i_name;
            mValue = i_value;
        }
    }
}