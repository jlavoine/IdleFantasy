using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestCannotTrainWhenUnitAlreadyTrainedToMax : TestTrainerAssignments {
        protected override IEnumerator RunTrainingTests() {
            yield return CannotTrainWhenUnitAlreadyTrainedToMax();
        }

        private IEnumerator CannotTrainWhenUnitAlreadyTrainedToMax() {
            SetTrainerCount( 1 );
            yield return mBackend.WaitUntilNotBusy();
            SetProgressData( 1, 1 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeAssignmentChange( 1 );

            FailTestIfClientInSync( "CannotTrainWhenUnitAlreadyTrainedToMax" );
        }
    }
}
