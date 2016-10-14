using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class CreateMapTest : IntegrationTestBase {

        public const int NUM_TESTS = 1;
        public const int TEST_LEVEL = 1;
        public const string TEST_WORLD = "TestWorld";

        protected override IEnumerator RunAllTests() {
            yield return InitConstants();

            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[BackendConstants.MAP_LEVEL] = TEST_LEVEL.ToString();
            testParams[BackendConstants.MAP_WORLD] = TEST_WORLD;
            testParams[BackendConstants.MAP_SIZE] = IntegrationTestUtils.DEFAULT_MAP_SIZE.ToString();

            for ( int i = 0; i < NUM_TESTS; ++i ) {
                mBackend.MakeCloudCall( CloudTestMethods.createMapForTesting.ToString(), testParams, CheckMap );
            }

            yield return mBackend.WaitUntilNotBusy();

            FailTestIfClientOutOfSync( "Create map test" );          
        }

        private void CheckMap( Dictionary<string, string> i_results ) {
            MapData mapData = JsonConvert.DeserializeObject<MapData>( i_results[BackendConstants.DATA] );

            new TestMapNames( mapData.Name, TEST_LEVEL );
            new TestMapAreas( mapData, IntegrationTestUtils.DEFAULT_MAP_SIZE );
            new TestUpcomingMaps( mapData, TEST_LEVEL );
            new TestMapMissions( mapData.Areas, mapData.AllModifications, TEST_LEVEL );   
        }
    }
}
