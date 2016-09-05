using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class TestMission : IntegrationTestBase {
        protected const string MISSION_WORLD = "Base";
        protected const int MISSION_GOLD_REWARD = 1000;
        private const int MISSION_INDEX = 0;

        protected abstract Dictionary<int, MissionTaskProposal> GetTaskProposals();
        protected abstract bool IsTestExpectedToFail();
        protected abstract string GetUnitProgressData();

        protected override IEnumerator RunAllTests() {
            bool completedMission = ShouldMarkMissionsComplete();

            yield return IntegrationTestUtils.SetPlayerCurrencyAndWait( 0 );
            yield return SetMissionDataOnServer();
            yield return SetMissionProgressOnServer( completedMission );
            yield return ClearUnitModifierData();
            yield return SetPlayerDataOnServer();
            yield return InitiateMissionComplete();

            if ( IsTestExpectedToFail() ) {
                FailTestIfClientInSync( GetType().ToString() );
            } else {
                FailTestIfClientOutOfSync( GetType().ToString() );
            }

            yield return RunOtherFailureChecks();
        }

        private IEnumerator SetMissionDataOnServer() {
            MapData map = CreateMapData();           
            string data = JsonConvert.SerializeObject( map );

            IntegrationTestUtils.SetReadOnlyData( "Map_" + MISSION_WORLD, data );

            yield return mBackend.WaitUntilNotBusy();
        }

        protected IEnumerator SetMissionProgressOnServer( bool i_bCompleted ) {
            List<SingleMissionProgress> listProgress = new List<SingleMissionProgress>();
            listProgress.Add( new SingleMissionProgress() { Completed = i_bCompleted } );

            WorldMissionProgress progressForWorld = new WorldMissionProgress();
            progressForWorld.World = MISSION_WORLD;
            progressForWorld.Missions = listProgress;

            Dictionary<string, WorldMissionProgress> dictData = new Dictionary<string, WorldMissionProgress>() { { MISSION_WORLD, progressForWorld } };

            IntegrationTestUtils.SetReadOnlyData( BackendConstants.MISSION_PROGRESS, JsonConvert.SerializeObject( dictData ) );

            yield return mBackend.WaitUntilNotBusy();
        }

        private IEnumerator ClearUnitModifierData() {
            IntegrationTestUtils.SetReadOnlyData( IntegrationTestUtils.SAVE_KEY_GUILDS, "{\"GUILD_1\":{\"Level\":0,\"Points\":0}}" );

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

        private MissionData CreateMissionData() {           
            MissionData testMissionData = new MissionData();

            testMissionData.MissionCategory = MISSION_WORLD;
            testMissionData.Index = MISSION_INDEX;
            testMissionData.GoldReward = MISSION_GOLD_REWARD;

            List<MissionTaskData> missionTasks = new List<MissionTaskData>();
            MissionTaskData taskOne = new MissionTaskData();
            taskOne.Index = 0;
            taskOne.PowerRequirement = 1000;
            taskOne.StatRequirement = "TEST_STAT_1";
            missionTasks.Add( taskOne );

            MissionTaskData taskTwo = new MissionTaskData();
            taskTwo.Index = 1;
            taskTwo.PowerRequirement = 2000;
            taskTwo.StatRequirement = "TEST_STAT_2";
            missionTasks.Add( taskTwo );

            testMissionData.Tasks = missionTasks;

            return testMissionData;
        }

        private IEnumerator SetPlayerDataOnServer() {
            string unitProgressData = GetUnitProgressData();
            IntegrationTestUtils.SetReadOnlyData( IntegrationTestUtils.SAVE_KEY_UNITS, unitProgressData );

            yield return mBackend.WaitUntilNotBusy();
        }

        private IEnumerator InitiateMissionComplete() {
            Dictionary<int, MissionTaskProposal> taskProposals = GetTaskProposals();
            BackendManager.Backend.CompleteMission( MISSION_WORLD, MISSION_INDEX, taskProposals );

            yield return mBackend.WaitUntilNotBusy();
        }

        #region Convenience methods for various tests
        protected string GetValidUnitProgressForMission() {
            return "{\"BASE_MELEE_1\":{\"Level\":1, \"Count\":10000, \"Trainers\":0, \"LastCountTime\":0}}";
        }

        protected Dictionary<int, MissionTaskProposal> GetValidMissionProposal() {
            Dictionary<int, MissionTaskProposal> taskProposals = new Dictionary<int, MissionTaskProposal>();
            taskProposals.Add( 0, new MissionTaskProposal( 0, "BASE_MELEE_1", 500 ) );
            taskProposals.Add( 1, new MissionTaskProposal( 1, "BASE_MELEE_1", 667 ) );

            return taskProposals;
        }

        protected virtual bool ShouldMarkMissionsComplete() {
            return false;
        }

        protected virtual IEnumerator RunOtherFailureChecks() {
            yield return null;
        }
        #endregion
    }
}