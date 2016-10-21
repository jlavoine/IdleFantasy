using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestCannotTrainWhenClientTimestampIsEarlier : TestTrainerAssignments {
        protected override int GetClientTimestampForChange() {
            return CURRENT_CLIENT_TIMESTAMP - 1;
        }

        protected override IEnumerator RunTrainingTests() {
            yield return TrainingFailsWithEarlyClientTimestamp();
        }

        private IEnumerator TrainingFailsWithEarlyClientTimestamp() {
            SetTrainerCount( 1 );
            yield return mBackend.WaitUntilNotBusy();
            SetProgressData( 1, 0 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeAssignmentChange( 1 );

            FailTestIfClientInSync( "TrainingFailsWithEarlyClientTimestamp" );
        }
    }
}
