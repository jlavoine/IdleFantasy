using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestMapNames : IntegrationTestBase {

        public const int NUM_TESTS = 10;
        public const int TEST_LEVEL = 1;
        public const string TEST_WORLD = "TestWorld";

        protected override IEnumerator RunAllTests() {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[BackendConstants.MAP_LEVEL] = TEST_LEVEL.ToString();
            testParams[BackendConstants.MAP_WORLD] = TEST_WORLD;

            for ( int i = 0; i < NUM_TESTS; ++i ) {
                mBackend.MakeCloudCall( CloudTestMethods.testMapNames.ToString(), testParams, CheckMapNames );
            }

            yield return mBackend.WaitUntilNotBusy();

            FailTestIfClientOutOfSync( "Map name test" );          
        }

        // how to improve this? Make the parts of the map name into an array?
        private void CheckMapNames( Dictionary<string, string> i_results ) {
            MapData data = JsonConvert.DeserializeObject<MapData>( i_results[BackendConstants.DATA] );

            if ( data.Prefix.PieceType != MapPieceTypes.Prefix ) {
                IntegrationTest.Fail( "Map name test failed: Prefix piece type was " + data.Prefix.PieceType + " and not " + MapPieceTypes.Prefix );
            }

            if ( !data.Prefix.LevelRestriction.DoesPass( TEST_LEVEL ) ) {
                IntegrationTest.Fail( "Map name test failed: Prefix level not valid" );
            }

            if ( data.Terrain.PieceType != MapPieceTypes.Terrain ) {
                IntegrationTest.Fail( "Map name test failed: Terrain piece type was " + data.Prefix.PieceType + " and not " + MapPieceTypes.Terrain );
            }

            if ( !data.Terrain.LevelRestriction.DoesPass( TEST_LEVEL ) ) {
                IntegrationTest.Fail( "Map name test failed: Terrain level not valid" );
            }

            if ( data.Suffix.PieceType != MapPieceTypes.Suffix ) {
                IntegrationTest.Fail( "Map name test failed: Suffix piece type was " + data.Prefix.PieceType + " and not " + MapPieceTypes.Suffix );
            }

            if ( !data.Suffix.LevelRestriction.DoesPass( TEST_LEVEL ) ) {
                IntegrationTest.Fail( "Map name test failed: Suffix level not valid" );
            }
        }
    }
}
