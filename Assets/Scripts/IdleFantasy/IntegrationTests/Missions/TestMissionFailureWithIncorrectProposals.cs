using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestMissionFailureWithIncorrectProposals : TestMission {
        protected override Dictionary<int, MissionTaskProposal> GetTaskProposals() {
            Dictionary<int, MissionTaskProposal> taskProposals = new Dictionary<int, MissionTaskProposal>();
            taskProposals.Add( 0, new MissionTaskProposal( 0, "BASE_MELEE_1", 500 ) );
            taskProposals.Add( 1, new MissionTaskProposal( 1, "BASE_MELEE_1", 1 ) );

            return taskProposals;
        }

        protected override string GetUnitProgressData() {
            return "{\"BASE_MELEE_1\":{\"Level\":1, \"Count\":10000, \"Trainers\":0, \"LastCountTime\":0}}";
        }

        protected override bool IsTestExpectedToFail() {
            return true;
        }
    }
}
