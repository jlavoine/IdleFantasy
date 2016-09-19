using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class GoodTravelSelection_Succeeds : TravelToSelectionTestBase {
        protected override Dictionary<string, WorldMissionProgress> GetMissionProgressForPlayer() {
            Dictionary<string, WorldMissionProgress> allProgress = new Dictionary<string, WorldMissionProgress>();
            WorldMissionProgress progress = new WorldMissionProgress();
            progress.World = BackendConstants.WORLD_BASE;
            progress.Missions = new List<SingleMissionProgress>();
            for ( int i = 0; i < TEST_SIZE; ++i ) {
                SingleMissionProgress singleProgress = new SingleMissionProgress();
                singleProgress.Completed = true;
                progress.Missions.Add( singleProgress );
            }

            allProgress.Add( progress.World, progress );
            return allProgress;
        }

        protected override IEnumerator RunOtherFailureChecks() {
            CheckNewMapIsCorrect();
            CheckMissionProgressIsClear();

            yield return mBackend.WaitUntilNotBusy();
        }

        private void CheckNewMapIsCorrect() {
            Dictionary<string, string> cloudParams = new Dictionary<string, string>() { { BackendConstants.SAVE_KEY, BackendConstants.MAP_BASE } };
            mBackend.MakeCloudCall( CloudTestMethods.getReadOnlyData.ToString(), cloudParams, ( results ) => {
                MapData newMap = JsonConvert.DeserializeObject<MapData>( results[BackendConstants.DATA] );
                CheckNewMapLevel( newMap );
                CheckNewMapName( newMap );
            } );
        }

        private void CheckNewMapLevel( MapData i_newMap ) {
            if ( i_newMap.MapLevel != MapData.MapLevel + 1 ) {
                IntegrationTest.Fail( "Next map level was not correct, expecting " + (MapData.MapLevel + 1) + " but got " + i_newMap.MapLevel );
            }
        }

        private void CheckNewMapName( MapData i_newMap ) {
            if ( i_newMap.Name.GetStringName() != MapData.UpcomingMaps[0].GetStringName() ) {
                IntegrationTest.Fail( "Next map name was not correct, expecting " + MapData.UpcomingMaps[0].GetStringName() + " but got " + i_newMap.Name.GetStringName() );
            }
        }

        private void CheckMissionProgressIsClear() {
            Dictionary<string, string> cloudParams = new Dictionary<string, string>() { { BackendConstants.SAVE_KEY, BackendConstants.MISSION_PROGRESS } };
            mBackend.MakeCloudCall( CloudTestMethods.getReadOnlyData.ToString(), cloudParams, ( results ) => {
                Dictionary<string, WorldMissionProgress> allMissionProgress = JsonConvert.DeserializeObject<Dictionary<string, WorldMissionProgress>>( results[BackendConstants.DATA] );
                WorldMissionProgress progress = allMissionProgress[BackendConstants.WORLD_BASE];
                foreach ( SingleMissionProgress singleProgress in progress.Missions ) {
                    if ( singleProgress.Completed != false ) {
                        IntegrationTest.Fail( "Expecting mission to not be complete, but it was." );
                    }
                }
            } );
        }

        protected override bool IsTestExpectedToFail() {
            return false;
        }
    }
}
