using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class TestMission : IntegrationTestBase {
        private const string MISSION_CATEGORY = "Test";
        private const int MISSION_INDEX = 0;

        protected abstract Dictionary<int, MissionTaskProposal> GetTaskProposals();
        protected abstract bool IsTestExpectedToFail();
        protected abstract string GetUnitProgressData();

        protected override IEnumerator RunAllTests() {
            yield return SetMissionDataOnServer();
            yield return ClearUnitModifierData();
            yield return SetPlayerDataOnServer();
            yield return InitiateMissionComplete();

            if ( IsTestExpectedToFail() ) {
                FailTestIfClientInSync( GetType().ToString() );
            }
        }

        private IEnumerator SetMissionDataOnServer() {
            List<MissionData> testMissionData = CreateMissionData();

            string data = JsonConvert.SerializeObject( testMissionData );
            IntegrationTestUtils.SetReadOnlyData( "Missions_" + MISSION_CATEGORY, data );

            yield return mBackend.WaitUntilNotBusy();
        }

        private IEnumerator ClearUnitModifierData() {
            IntegrationTestUtils.SetReadOnlyData( IntegrationTestUtils.SAVE_KEY_GUILDS, "{\"GUILD_1\":{\"Level\":0,\"Points\":0}}" );

            yield return mBackend.WaitUntilNotBusy();
        }

        private List<MissionData> CreateMissionData() {           
            MissionData testMissionData = new MissionData();

            testMissionData.MissionCategory = MISSION_CATEGORY;
            testMissionData.Index = MISSION_INDEX;

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

            return new List<MissionData>() { testMissionData };
        }

        private IEnumerator SetPlayerDataOnServer() {
            string unitProgressData = GetUnitProgressData();
            IntegrationTestUtils.SetReadOnlyData( IntegrationTestUtils.SAVE_KEY_UNITS, unitProgressData );

            yield return mBackend.WaitUntilNotBusy();
        }

        private IEnumerator InitiateMissionComplete() {
            Dictionary<int, MissionTaskProposal> taskProposals = GetTaskProposals();
            BackendManager.Backend.CompleteMission( MISSION_CATEGORY, MISSION_INDEX, taskProposals );

            yield return mBackend.WaitUntilNotBusy();
        }
    }
}