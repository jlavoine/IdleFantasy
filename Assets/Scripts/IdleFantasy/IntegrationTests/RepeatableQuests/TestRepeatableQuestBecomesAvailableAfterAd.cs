using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestRepeatableQuestBecomesAvailableAfterAd : IntegrationTestBase {

        protected override IEnumerator RunAllTests() {
            yield return SetInitialProgress();
            yield return SendWatchAdToServer();
            yield return AssertRepeatableQuestIsAvailable();
        }

        private IEnumerator SetInitialProgress() {
            RepeatableQuestProgress questProgress = new RepeatableQuestProgress();
            questProgress.CompletedCount = 0;
            questProgress.CurrentlyAvailable = false;
            questProgress.World = BackendConstants.WORLD_BASE;

            Dictionary<string, RepeatableQuestProgress> allQuestProgress = new Dictionary<string, RepeatableQuestProgress>();
            allQuestProgress[BackendConstants.WORLD_BASE] = questProgress;
            string data = JsonConvert.SerializeObject( allQuestProgress );

            IntegrationTestUtils.SetReadOnlyData( BackendConstants.REPEATABLE_QUEST_PROGRESS, data );

            yield return mBackend.WaitUntilNotBusy();
        }

        private IEnumerator SendWatchAdToServer() {
            Dictionary<string, string> cloudParams = new Dictionary<string, string>();
            cloudParams.Add( BackendConstants.MAP_WORLD, BackendConstants.WORLD_BASE );
            BackendManager.Backend.MakeCloudCall( BackendConstants.WATCHED_REPEATABLE_QUEST_AD, cloudParams, null );

            yield return mBackend.WaitUntilNotBusy();
        }

        private IEnumerator AssertRepeatableQuestIsAvailable() {
            mBackend.GetPlayerDataDeserialized<Dictionary<string, RepeatableQuestProgress>>( BackendConstants.REPEATABLE_QUEST_PROGRESS, ( allProgressData ) => {
                RepeatableQuestProgress progress = allProgressData[BackendConstants.WORLD_BASE];
                if ( progress.CurrentlyAvailable != true ) {
                    IntegrationTest.Fail( "Repeatable quest progress did not set to true after watching ad." );
                }
            } );

            yield return mBackend.WaitUntilNotBusy();
        }                
    }
}
