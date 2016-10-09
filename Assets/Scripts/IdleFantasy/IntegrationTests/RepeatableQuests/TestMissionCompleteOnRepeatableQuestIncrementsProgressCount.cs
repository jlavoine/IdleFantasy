using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestMissionCompleteOnRepeatableQuestIncrementsProgressCount : TestNormalMissionSuccessForRepeatableQuestMission {
        protected override IEnumerator RunOtherFailureChecks() {
            yield return base.RunOtherFailureChecks();

            yield return FailIfProgressCountNotIncremented();
        }

        private IEnumerator FailIfProgressCountNotIncremented() {
            mBackend.GetPlayerDataDeserialized<Dictionary<string, RepeatableQuestProgress>>( BackendConstants.REPEATABLE_QUEST_PROGRESS, ( allProgressData ) => {
                RepeatableQuestProgress progress = allProgressData[MISSION_WORLD];
                if ( progress.CompletedCount != 1 ) {
                    IntegrationTest.Fail( "Repeatable quest progress did not increment to 1, instead was " + progress.CompletedCount );
                }
            } );

            yield return mBackend.WaitUntilNotBusy();
        }
    }
}