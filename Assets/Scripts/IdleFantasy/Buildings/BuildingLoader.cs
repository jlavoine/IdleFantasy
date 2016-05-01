using System.Collections.Generic;
using MyLibrary;
using Newtonsoft.Json;
using PlayFab;

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

        private static void Test( string test ) {
            //Dictionary<string, SimpleBuildingData> testdict = JsonConvert.DeserializeObject<Dictionary<string, SimpleBuildingData>>( test );
            mData = PlayFab.SimpleJson.DeserializeObject<Dictionary<string, BuildingData>>( test );

            //Debug.LogError( "TEST: " + testdict[ "BASE_BUILDING_1"].Unit );
            Debug.LogError(GetData("BASE_BUILDING_1").LevelUpgrade.PropertyName);
        }

        private static void LoadData() {
            if ( mBackend != null ) {
                mBackend.GetAllDataForClass( "Buildings", Test );
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