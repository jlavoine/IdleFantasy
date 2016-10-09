using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestMissionFailBecauseRepeatableQuestNotAvailable : RepeatableQuestMissionTestBase {

        protected override Dictionary<int, MissionTaskProposal> GetTaskProposals() {
            return GetValidMissionProposal();
        }

        protected override string GetUnitProgressData() {
            return GetValidUnitProgressForMission();
        }

        protected override bool ShouldQuestBeAvailable() {
            return false;
        }

        protected override bool IsTestExpectedToFail() {
            return true;
        }
    }
}
