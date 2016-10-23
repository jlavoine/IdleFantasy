using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using MyLibrary;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class TestMission : IntegrationTestBase {
        protected const string MISSION_WORLD = "Base";
        protected const string UNIT_ID = "BASE_WARRIOR_1";
        protected const string BUILDING_ID = "BASE_WARRIOR_BUILDING_1";
        protected const int MISSION_GOLD_REWARD = 1000;
        protected const int MISSIONS_DONE_COUNT = 0;
        protected const int TASK_1_PROPOSAL_COUNT = 250;
        protected const int TASK_2_PROPOSAL_COUNT = 667;
        protected const float VALID_MISSION_PROGRESS_UNIT_COUNT = 9000.5f;

        private const int MISSION_INDEX = 0;                               

        protected abstract Dictionary<int, MissionTaskProposal> GetTaskProposals();
        protected abstract string GetUnitProgressData();
        protected abstract IEnumerator SetMissionDataOnServer();
        protected abstract string GetMissionCategory();

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

        protected MissionData CreateMissionData() {           
            MissionData testMissionData = new MissionData();

            testMissionData.MissionCategory = GetMissionCategory();
            testMissionData.MissionWorld = MISSION_WORLD;
            testMissionData.Index = MISSION_INDEX;
            testMissionData.GoldReward = MISSION_GOLD_REWARD;

            List<MissionTaskData> missionTasks = new List<MissionTaskData>();
            MissionTaskData taskOne = new MissionTaskData();
            taskOne.Index = 0;
            taskOne.PowerRequirement = 1000;
            taskOne.StatRequirement = "BASE_STAT_1";
            missionTasks.Add( taskOne );

            MissionTaskData taskTwo = new MissionTaskData();
            taskTwo.Index = 1;
            taskTwo.PowerRequirement = 2000;
            taskTwo.StatRequirement = "BASE_STAT_2";
            missionTasks.Add( taskTwo );

            testMissionData.Tasks = missionTasks;

            return testMissionData;
        }

        private IEnumerator SetPlayerDataOnServer() {
            SetUnitProgressData();
            SetBuildingProgressData();
            SetGameMetricData();

            yield return mBackend.WaitUntilNotBusy();
        }

        private void SetUnitProgressData() {
            string unitProgressData = GetUnitProgressData();
            IntegrationTestUtils.SetReadOnlyData( IntegrationTestUtils.SAVE_KEY_UNITS, unitProgressData );
        }

        private void SetBuildingProgressData() {
            string progressData = "{\"" + BUILDING_ID + "\":{\"Level\":100}}";
            IntegrationTestUtils.SetReadOnlyData( IntegrationTestUtils.SAVE_KEY_BUILDINGS, progressData );
        }

        private void SetGameMetricData() {
            GameMetrics metrics = new GameMetrics();
            metrics.Metrics = new Dictionary<string, int>();
            metrics.Metrics.Add( GameMetricsList.TOTAL_MISSIONS_DONE, MISSIONS_DONE_COUNT );

            IntegrationTestUtils.SetReadOnlyData( IntegrationTestUtils.SAVE_KEY_METRICS, JsonConvert.SerializeObject( metrics ) );
        }

        private IEnumerator InitiateMissionComplete() {
            Dictionary<int, MissionTaskProposal> taskProposals = GetTaskProposals();
            BackendManager.Backend.CompleteMission( CreateMissionData(), taskProposals );

            yield return mBackend.WaitUntilNotBusy();
        }

        #region Convenience methods for various tests
        protected string GetValidUnitProgressForMission() {
            return "{\"" + UNIT_ID + "\":{\"Level\":1, \"Count\":" + VALID_MISSION_PROGRESS_UNIT_COUNT.ToString() + ", \"Trainers\":0, \"LastCountTime\":" + long.MaxValue + "}}";
        }

        protected Dictionary<int, MissionTaskProposal> GetValidMissionProposal() {
            Dictionary<int, MissionTaskProposal> taskProposals = new Dictionary<int, MissionTaskProposal>();
            taskProposals.Add( 0, new MissionTaskProposal( 0, UNIT_ID, TASK_1_PROPOSAL_COUNT ) );
            taskProposals.Add( 1, new MissionTaskProposal( 1, UNIT_ID, TASK_2_PROPOSAL_COUNT ) );

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