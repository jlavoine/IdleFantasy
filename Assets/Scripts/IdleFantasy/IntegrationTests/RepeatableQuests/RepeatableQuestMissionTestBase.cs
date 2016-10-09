using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class RepeatableQuestMissionTestBase : TestMission {
        protected abstract bool ShouldQuestBeAvailable();

        protected override string GetMissionCategory() {
            return BackendConstants.MISSION_TYPE_REPEATABLE_QUEST;
        }

        protected override IEnumerator SetMissionDataOnServer() {
            RepeatableQuestProgress questProgress = new RepeatableQuestProgress();
            questProgress.CompletedCount = 0;
            questProgress.CurrentlyAvailable = ShouldQuestBeAvailable();
            questProgress.World = MISSION_WORLD;
            questProgress.Mission = CreateMissionData();

            Dictionary<string, RepeatableQuestProgress> allQuestProgress = new Dictionary<string, RepeatableQuestProgress>();
            allQuestProgress[MISSION_WORLD] = questProgress;
            string data = JsonConvert.SerializeObject( allQuestProgress );

            IntegrationTestUtils.SetReadOnlyData( BackendConstants.REPEATABLE_QUEST_PROGRESS, data );

            yield return mBackend.WaitUntilNotBusy();
        }
    }
}