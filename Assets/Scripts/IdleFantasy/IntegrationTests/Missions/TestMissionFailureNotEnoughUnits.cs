using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestMissionFailureNotEnoughUnits : TestMission {
        protected override Dictionary<int, MissionTaskProposal> GetTaskProposals() {
            return GetValidMissionProposal();
        }

        protected override string GetUnitProgressData() {
            return "{\"BASE_MELEE_1\":{\"Level\":1, \"Count\":0, \"Trainers\":0, \"LastCountTime\":0}}";
        }

        protected override bool IsTestExpectedToFail() {
            return true;
        }
    }
}
