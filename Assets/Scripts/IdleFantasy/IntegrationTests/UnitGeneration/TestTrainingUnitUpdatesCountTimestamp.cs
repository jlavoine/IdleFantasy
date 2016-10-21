using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestTrainingUnitUpdatesCountTimestamp : TestUnitGeneration {
        private const int LOGGED_IN_TIME = 0;

        protected override IEnumerator RunAllTests() {
            yield return RunTest();
        }

        private IEnumerator RunTest() {
            yield return SetDataForTestPrep();
            yield return IntegrationTestUtils.SetInternalData( BackendConstants.LOGGED_IN_TIME, LOGGED_IN_TIME.ToString() );
            yield return IntegrationTestUtils.TrainUnit( UNIT_BEING_COUNTED, 1 );

            yield return FailTestIfLastCountTimeNotUpdated();
        }
    }
}

