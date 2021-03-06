﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestMapAreas {
        public TestMapAreas( MapData i_data, int i_size) {
            CheckAreasSize( i_data, i_size );
            CheckAreaTypeMinimums( i_data );
        }

        private void CheckAreasSize( MapData i_mapData, int i_size ) {
            if ( i_mapData.Areas.Count != i_size ) {
                IntegrationTest.Fail( "Test map areas fail: Expecting " + i_size + " areas but there were " + i_mapData.Areas.Count );
            }
        }

        // this method is ugly...this whole thing is not great or flexible
        private void CheckAreaTypeMinimums( MapData i_mapData ) {
            BackendManager.Backend.MakeCloudCall( CloudTestMethods.getDefaultMapAreaWeights.ToString(), null, ( results ) => {
                List<MapModification> defaultWeights = JsonConvert.DeserializeObject<List<MapModification>>( results[BackendConstants.DATA] );
                defaultWeights = RemoveNonMinimumsFromDefaults( defaultWeights );

                // now change the weights based on the modifications of the map data
                defaultWeights = ModifyDefaultWeightsFromMapPieces( defaultWeights, i_mapData.Name.Prefix );
                defaultWeights = ModifyDefaultWeightsFromMapPieces( defaultWeights, i_mapData.Name.Terrain );
                defaultWeights = ModifyDefaultWeightsFromMapPieces( defaultWeights, i_mapData.Name.Suffix );

                // decrement the minimum for a weight when it shows up -- NOT SAFE IF SOME TYPES NOT REPRESENTED
                foreach ( MapAreaData areaData in i_mapData.Areas ) {
                    defaultWeights[(int) areaData.AreaType].Amount--;
                }

                foreach ( MapModification defaultWeight in defaultWeights ) {
                    if ( IsMinimumKey( defaultWeight.Key ) && defaultWeight.Amount > 0 ) {
                        IntegrationTest.Fail( "Test map areas failed: Minimum not met for area type " + defaultWeight.Key + "(" + defaultWeight.Amount + ")" );
                    }
                }
            } );
        }

        // checks to see if the incoming key is a "minimum" key, i.e. that the modification it represents is a what dicates
        // the minimum # of areas for a given type
        private bool IsMinimumKey( string i_key ) {
            return i_key == BackendConstants.COMBAT_MIN || i_key == BackendConstants.EXPLORE_MIN || i_key == BackendConstants.MISC_MIN;
        }

        // this is a total hack. the default list of map modifications contains things other than area minimums, which is what 
        // we are testing here. this method recreates a list and adds only the minimums to it. the order is presume to be
        // combat, explore, and then misc. very fragile...sorry future coder...
        private List<MapModification> RemoveNonMinimumsFromDefaults( List<MapModification> io_weights ) {
            List<MapModification> newList = new List<MapModification>();

            foreach ( MapModification modification in io_weights ) {
                if ( IsMinimumKey( modification.Key ) ) {
                    newList.Add( modification );
                }
            }

            return newList;
        }

        private List<MapModification> ModifyDefaultWeightsFromMapPieces( List<MapModification> io_weights, MapPieceData i_pieceData ) {
            foreach ( MapModification modifier in i_pieceData.Modifications ) {
                if ( modifier.Key == BackendConstants.COMBAT_MIN ) {
                    io_weights[(int) MapAreaTypes.Combat].Amount += (int) modifier.Amount;
                }
                else if ( modifier.Key == BackendConstants.EXPLORE_MIN ) {
                    io_weights[(int) MapAreaTypes.Explore].Amount += (int) modifier.Amount;
                }
                else if ( modifier.Key == BackendConstants.MISC_MIN ) {
                    io_weights[(int) MapAreaTypes.Misc].Amount += (int) modifier.Amount;
                }
            }

            return io_weights;
        }
    }
}
