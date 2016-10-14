using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class BadTravelToSelection_Fails : TravelToSelectionTestBase {
        protected override Dictionary<string, WorldMissionProgress> GetMissionProgressForPlayer() {
            Dictionary<string, WorldMissionProgress> allProgress = new Dictionary<string, WorldMissionProgress>();
            WorldMissionProgress progress = new WorldMissionProgress();
            progress.World = BackendConstants.WORLD_BASE;
            progress.Missions = new List<SingleMissionProgress>();
            for ( int i = 0; i < IntegrationTestUtils.DEFAULT_MAP_SIZE; ++i ) {
                SingleMissionProgress singleProgress = new SingleMissionProgress();
                singleProgress.Completed = false;
                progress.Missions.Add( singleProgress );
            }

            allProgress.Add( progress.World, progress );
            return allProgress;
        }

        protected override IEnumerator RunOtherFailureChecks() {
            yield return null;
        }

        protected override bool IsTestExpectedToFail() {
            return true;
        }
    }
}
