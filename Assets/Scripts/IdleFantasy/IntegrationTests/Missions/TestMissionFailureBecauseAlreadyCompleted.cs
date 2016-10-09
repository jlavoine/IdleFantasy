using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestMissionFailureBecauseAlreadyCompleted : MapMissionTestBase {
        protected override Dictionary<int, MissionTaskProposal> GetTaskProposals() {
            return GetValidMissionProposal();
        }

        protected override bool ShouldMarkMissionsComplete() {
            return true;
        }

        protected override string GetUnitProgressData() {
            return GetValidUnitProgressForMission();
        }

        protected override bool IsTestExpectedToFail() {
            return true;
        }
    }
}
