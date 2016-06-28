using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestTrainingUnitUpdatesCountTimestamp : TestUnitGeneration {
        protected override IEnumerator RunAllTests() {
            yield return RunTest();
        }

        private IEnumerator RunTest() {
            yield return SetDataForTestPrep();
            yield return IntegrationTestUtils.TrainUnit( UNIT_BEING_COUNTED, 1 );

            yield return FailTestIfLastCountTimeNotUpdated();
        }
    }
}

