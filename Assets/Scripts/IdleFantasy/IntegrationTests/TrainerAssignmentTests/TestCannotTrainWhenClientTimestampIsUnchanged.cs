using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestCannotTrainWhenClientTimestampIsUnchanged : TestTrainerAssignments {
        protected override int GetClientTimestampForChange() {
            return CURRENT_CLIENT_TIMESTAMP;
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

            FailTestIfClientInSync( "TestCannotTrainWhenClientTimestampIsUnchanged" );
        }
    }
}
