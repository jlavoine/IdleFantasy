using System.Collections.Generic;
using MyLibrary;
using Newtonsoft.Json;

namespace IdleFantasy {
    public static class BuildingLoader {
        public const string TEST_BUILDING = "TEST_BUILDING";

        private static Dictionary<string, BuildingData> mData = null;
        private static IBackend mBackend;

        public static BuildingData GetData( string i_key ) {
            if ( mData == null )
                LoadData();

            if ( mData.ContainsKey( i_key ) ) {
                return mData[i_key];
            } else {
                Debug.LogError( "Tried to load " + i_key + " but no data!" );
                return new BuildingData();
            }
        }

        public static void Init( IBackend i_backend ) {
            mBackend = i_backend;
            LoadData();
        }

        private static void DeserializeData( string i_data ) {
            mData = JsonConvert.DeserializeObject<Dictionary<string, BuildingData>>( i_data );            
        }

        private static void LoadData() {
            if ( mBackend != null ) {
                //mBackend.GetAllTitleDataForClass( "Buildings", DeserializeData );
            }
            else {
                mData = new Dictionary<string, BuildingData>();
                DataUtils.LoadData<BuildingData>( mData, "Buildings" );
            }

            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //settings.TypeNameHandling = TypeNameHandling.All;
            //settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
            //string test = JsonConvert.SerializeObject( m_listCharacters, Formatting.Indented, settings );
            //Debug.Log( test );
        }
    }
}