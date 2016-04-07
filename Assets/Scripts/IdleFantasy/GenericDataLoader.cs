using System.Collections.Generic;
using System.Collections;
using MyLibrary;

namespace IdleFantasy {
    public static class GenericDataLoader {
        // types of data
        public const string BUILDINGS = "Buildings";
        public const string UNITS = "Units";

        // convenient constants for testing
        public const string TEST_BUILDING = "TEST_BUILDING";
        public const string TEST_UNIT = "TEST_UNIT";

        private static Dictionary<string, Hashtable> mData = new Dictionary<string, Hashtable>();

        public static T GetData<T>( string i_type, string i_key ) where T : GenericData {
            if ( !mData.ContainsKey( i_type ) ) {
                LoadDataOfType<T>( i_type );
            }

            Hashtable dataOfType = mData[i_type];

            if ( dataOfType.ContainsKey( i_key ) ) {
                return (T)dataOfType[i_key];
            }
            else {
                Debug.LogError( "Tried to load " + i_key + " from " + i_type + " but no data!" );
                return default( T );
            }
        }

        private static void LoadDataOfType<T>( string i_type ) where T : GenericData {
            Hashtable data = new Hashtable();
            DataUtils.LoadDataAsHash<T>( data, i_type );
            mData[i_type] = data;

            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //settings.TypeNameHandling = TypeNameHandling.All;
            //settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
            //string test = JsonConvert.SerializeObject( m_listCharacters, Formatting.Indented, settings );
            //Debug.Log( test );
        }
    }
}