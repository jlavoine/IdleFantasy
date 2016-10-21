using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestCannotTrainWithNoTrainers : TestTrainerAssignments {
        protected override IEnumerator RunTrainingTests() {
            yield return CannotTrainWithNoTrainers();
        }

        private IEnumerator CannotTrainWithNoTrainers() {
            SetTrainerCount( 0 );
            yield return mBackend.WaitUntilNotBusy();
            SetProgressData( 1, 0 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeAssignmentChange( 1 );

            FailTestIfClientInSync( "CannotTrainWithNoTrainers" );
        }
    }
}