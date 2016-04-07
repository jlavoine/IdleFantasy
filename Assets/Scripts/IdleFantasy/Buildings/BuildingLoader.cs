using System.Collections.Generic;

namespace IdleFantasy {
    public static class BuildingLoader {
        public const string TEST_BUILDING = "TEST_BUILDING";

        private static Dictionary<string, BuildingData> mData = null;

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

        private static void LoadData() {
            mData = new Dictionary<string, BuildingData>();
            DataUtils.LoadData<BuildingData>( mData, "Buildings" );

            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //settings.TypeNameHandling = TypeNameHandling.All;
            //settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
            //string test = JsonConvert.SerializeObject( m_listCharacters, Formatting.Indented, settings );
            //Debug.Log( test );
        }
    }
}