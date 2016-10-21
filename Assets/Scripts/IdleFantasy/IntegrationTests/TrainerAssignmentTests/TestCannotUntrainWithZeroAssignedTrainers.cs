using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestCannotUntrainWithZeroAssignedTrainers : TestTrainerAssignments {
        protected override IEnumerator RunTrainingTests() {
            yield return CannotUntrainWithZeroAssignedTrainers();
        }

        private IEnumerator CannotUntrainWithZeroAssignedTrainers() {
            SetTrainerCount( 1 );
            yield return mBackend.WaitUntilNotBusy();
            SetProgressData( 1, 0 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeAssignmentChange( -1 );

            FailTestIfClientInSync( "CannotUntrainWithZeroAssignedTrainers" );
        }
    }
}

