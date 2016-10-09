using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestMissionFailureWithIncorrectProposals : MapMissionTestBase {
        protected override Dictionary<int, MissionTaskProposal> GetTaskProposals() {
            Dictionary<int, MissionTaskProposal> taskProposals = new Dictionary<int, MissionTaskProposal>();
            taskProposals.Add( 0, new MissionTaskProposal( 0, "BASE_MELEE_1", 500 ) );
            taskProposals.Add( 1, new MissionTaskProposal( 1, "BASE_MELEE_1", 1 ) );

            return taskProposals;
        }

        protected override string GetUnitProgressData() {
            return GetValidUnitProgressForMission();
        }

        protected override bool IsTestExpectedToFail() {
            return true;
        }
    }
}
