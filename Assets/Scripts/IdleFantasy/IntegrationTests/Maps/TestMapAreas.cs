using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestMapAreas : IntegrationTestBase {

        public const int NUM_TESTS = 1;
        public const int TEST_LEVEL = 1;
        public const int TEST_SIZE = 36;
        public const string TEST_WORLD = "TestWorld";

        protected override IEnumerator RunAllTests() {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[BackendConstants.MAP_LEVEL] = TEST_LEVEL.ToString();
            testParams[BackendConstants.MAP_WORLD] = TEST_WORLD;
            testParams[BackendConstants.MAP_SIZE] = TEST_SIZE.ToString();

            for ( int i = 0; i < NUM_TESTS; ++i ) {
                mBackend.MakeCloudCall( CloudTestMethods.testMapAreas.ToString(), testParams, CheckAreas );
            }

            yield return mBackend.WaitUntilNotBusy();

            FailTestIfClientOutOfSync( "Map area test" );          
        }

        private void CheckAreas( Dictionary<string, string> i_results ) {
            MapData mapData = JsonConvert.DeserializeObject<MapData>( i_results[BackendConstants.DATA] );

            CheckAreasSize( mapData );
            CheckAreaTypeMinimums( mapData );
        }

        private void CheckAreasSize( MapData i_mapData ) {
            if ( i_mapData.Areas.Count != TEST_SIZE ) {
                IntegrationTest.Fail( "Test map areas fail: Expecting " + TEST_SIZE + " areas but there were " + i_mapData.Areas.Count );
            }
        }

        // this method is ugly...
        private void CheckAreaTypeMinimums( MapData i_mapData ) {
            mBackend.MakeCloudCall( CloudTestMethods.getDefaultMapAreaWeights.ToString(), null, ( results ) => {
                List<DefaultMapAreaWeight> defaultWeights = JsonConvert.DeserializeObject<List<DefaultMapAreaWeight>>( results[BackendConstants.DATA] );

                // now change the weights based on the modifications of the map data
                defaultWeights = ModifyDefaultWeightsFromMapPieces( defaultWeights, i_mapData.Prefix );
                defaultWeights = ModifyDefaultWeightsFromMapPieces( defaultWeights, i_mapData.Terrain );
                defaultWeights = ModifyDefaultWeightsFromMapPieces( defaultWeights, i_mapData.Suffix );

                // decrement the minimum for a weight when it shows up -- NOT SAFE IF SOME TYPES NOT REPRESENTED
                foreach (MapAreaData areaData in i_mapData.Areas ) {
                    defaultWeights[(int)areaData.AreaType].Minimum--;
                }

                foreach ( DefaultMapAreaWeight defaultWeight in defaultWeights ) {
                    if ( defaultWeight.Minimum > 0 ) {
                        IntegrationTest.Fail( "Test map areas failed: Minimum not met for area type " + defaultWeight.AreaType );
                    }
                }
            } );
        }

        private List<DefaultMapAreaWeight> ModifyDefaultWeightsFromMapPieces( List<DefaultMapAreaWeight> io_weights, MapPieceData i_pieceData ) {
            foreach ( MapModifier modifier in i_pieceData.Modifications ) {
                foreach ( MapModification modification in modifier.Modifications ) {
                    if ( modification.Key == BackendConstants.MINIMUM ) {
                        io_weights[(int) modifier.ModifiesType].Minimum += (int) modification.Amount;
                    }
                }
            }

            return io_weights;
        }
    }
}
