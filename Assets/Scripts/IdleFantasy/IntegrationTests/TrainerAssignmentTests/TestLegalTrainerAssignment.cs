using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestLegalTrainerAssignment : TestTrainerAssignments {
        protected override IEnumerator RunTrainingTests() {
            yield return TestLegalAssignment();
        }

        private IEnumerator TestLegalAssignment() {
            SetTrainerCount( 1 );
            yield return mBackend.WaitUntilNotBusy();
            SetProgressData( 1, 0 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeAssignmentChange( 1 );

            FailTestIfReturnedCallDoesNotEqual( CloudTestMethods.getAvailableTrainers.ToString(), 0 );
            FailTestIfAssignedTrainersDoesNotEqual( 1 );
            FailIfClientTimestampNotUpdated();

            yield return mBackend.WaitUntilNotBusy();
        }

        private void FailIfClientTimestampNotUpdated() {
            GetProgressData<UnitProgress>( GenericDataLoader.UNITS, UNIT_ID, ( result ) => {
                if ( result.ClientTimestamp != CLIENT_TIMESTAMP ) {
                    IntegrationTest.Fail( "Expecting client timestamp to be " + CLIENT_TIMESTAMP + " but was " + result.ClientTimestamp );
                }
            } );
        }
    }
}