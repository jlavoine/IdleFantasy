using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestMissionCompleteOnRepeatableQuestCreatesNewMission : TestNormalMissionSuccessForRepeatableQuestMission {
        protected override IEnumerator RunOtherFailureChecks() {
            yield return base.RunOtherFailureChecks();

            yield return FailIfAvailabilityNotFalse();
        }

        private IEnumerator FailIfAvailabilityNotFalse() {
            mBackend.GetPlayerDataDeserialized<Dictionary<string, RepeatableQuestProgress>>( BackendConstants.REPEATABLE_QUEST_PROGRESS, ( allProgressData ) => {
                RepeatableQuestProgress progress = allProgressData[MISSION_WORLD];
                if ( progress.CurrentlyAvailable != false ) {
                    IntegrationTest.Fail( "Repeatable quest progress did not reset availability after completion." );
                }
            } );

            yield return mBackend.WaitUntilNotBusy();
        }
    }
}
