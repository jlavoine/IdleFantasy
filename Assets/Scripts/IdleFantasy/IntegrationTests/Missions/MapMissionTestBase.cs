using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class MapMissionTestBase : TestMission {

        protected override IEnumerator RunOtherFailureChecks() {
            yield return base.RunOtherFailureChecks();

            yield return FailIfMissionsCompletedMetricNotCorrect();
        }

        private IEnumerator FailIfMissionsCompletedMetricNotCorrect() {
            int correctMetric = IsTestExpectedToFail() ? MISSIONS_DONE_COUNT : MISSIONS_DONE_COUNT + 1;

            Dictionary<string, string> cloudParams = new Dictionary<string, string>() { { BackendConstants.KEY, GameMetricsList.TOTAL_MISSIONS_DONE } };
            FailTestIfReturnedCallDoesNotEqual( CloudTestMethods.getGameMetric.ToString(), correctMetric, cloudParams );

            yield return mBackend.WaitUntilNotBusy();
        }

        protected override IEnumerator SetMissionDataOnServer() {
            MapData map = CreateMapData();
            string data = JsonConvert.SerializeObject( map );

            IntegrationTestUtils.SetReadOnlyData( "Map_" + MISSION_WORLD, data );

            yield return mBackend.WaitUntilNotBusy();
        }

        private MapData CreateMapData() {
            MapData map = new MapData();
            map.World = MISSION_WORLD;
            map.MapSize = 1;
            map.Areas = new List<MapAreaData>();

            MapAreaData area0 = new MapAreaData();
            area0.Index = 0;
            area0.Mission = CreateMissionData();
            map.Areas.Add( area0 );

            return map;
        }

        protected override string GetMissionCategory() {
            return BackendConstants.MISSION_TYPE_MAP;
        }
    }
}
