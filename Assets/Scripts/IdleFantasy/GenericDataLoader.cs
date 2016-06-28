using System.Collections.Generic;
using System.Collections;
using MyLibrary;
using Newtonsoft.Json;

namespace IdleFantasy {
    public static class GenericDataLoader {
        // types of data
        public const string BUILDINGS = "Buildings";
        public const string UNITS = "Units";
        public const string GUILDS = "Guilds";

        // convenient constants for testing
        public const string TEST_BUILDING = "BASE_BUILDING_1";
        public const string TEST_UNIT = "BASE_MELEE_1";
        public const string TEST_UNIT_2 = "BASE_MELEE_2";

        private static IBasicBackend mBackend;

        private static Dictionary<string, Hashtable> mData = new Dictionary<string, Hashtable>();

        public static void Init( IBasicBackend i_backend ) {
            mBackend = i_backend;
        }

        public static T GetData<T>( string i_key ) where T : GenericData {
            string dataType = GetDataType<T>();

            if ( !mData.ContainsKey( dataType ) ) {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Fatal, "Trying to get data of unloaded type " + dataType, "" );
                return default ( T );
            }

            Hashtable dataOfType = mData[dataType]; 

            if ( dataOfType.ContainsKey( i_key ) ) {
                return (T)dataOfType[i_key];
            }
            else {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Fatal, "Tried to load " + i_key + " from " + dataType + " but there was no data!", "" );
                return default( T );
            }
        }

        private static string GetDataType<T>() {
            if ( typeof( T ) == typeof( GuildData ) ) {
                return GUILDS;
            } else if ( typeof( T ) == typeof( BuildingData ) ) {
                return BUILDINGS;
            } else if ( typeof( T ) == typeof( UnitData ) ) {
                return UNITS;
            } else {
                return "Illegal data type";
            }
        }

        private static Dictionary<string, T> DeserializeData<T>( string i_data, string i_className ) {            
            Dictionary<string, T> dataAsDictionary = JsonConvert.DeserializeObject<Dictionary<string, T>>( i_data );

            return dataAsDictionary;
        }

        private static void StoreDictionaryDataAsHash<T>( Dictionary<string, T> i_data, string i_className ) {
            Hashtable hashOfData = new Hashtable();

            foreach ( KeyValuePair<string, T> pair in i_data ) {
                hashOfData[pair.Key] = pair.Value;
            }

            mData[i_className] = hashOfData;
        }

        public static void LoadDataOfClass<T>( string i_className ) where T : GenericData {
            if ( mBackend == null ) {
                Debug.LogError( "Generic data loader was not inited!" );
            }

            mBackend.GetAllTitleDataForClass( i_className, ( data ) => {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Got title data for " + i_className, "" );

                Dictionary<string, T> dataAsDictionary = DeserializeData<T>( data, i_className );
                StoreDictionaryDataAsHash<T>( dataAsDictionary, i_className );
            } );
           
            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //settings.TypeNameHandling = TypeNameHandling.All;
            //settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
            //string test = JsonConvert.SerializeObject( m_listCharacters, Formatting.Indented, settings );
            //Debug.Log( test );
        }
    }
}