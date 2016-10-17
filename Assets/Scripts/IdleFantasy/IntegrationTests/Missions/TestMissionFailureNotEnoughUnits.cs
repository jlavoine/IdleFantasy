using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestMissionFailureNotEnoughUnits : MapMissionTestBase {
        protected override Dictionary<int, MissionTaskProposal> GetTaskProposals() {
            return GetValidMissionProposal();
        }

        protected override string GetUnitProgressData() {
            return "{\"BASE_WARRIOR_1\":{\"Level\":1, \"Count\":0, \"Trainers\":0, \"LastCountTime\":" + long.MaxValue + "}}";
        }

        protected override bool IsTestExpectedToFail() {
            return true;
        }
    }
}
