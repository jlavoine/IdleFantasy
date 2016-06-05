using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestTrainingUnitUpdatesCountTimestamp : TestUnitGeneration {
        protected override IEnumerator RunAllTests() {
            yield return RunTest();
        }

        private IEnumerator RunTest() {
            yield return SetDataForTestPrep();
            yield return TrainUnit();

            yield return FailTestIfLastCountTimeNotUpdated();
        }

        private IEnumerator TrainUnit() {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[TARGET_ID] = UNIT_BEING_COUNTED;
            testParams[CLASS] = GenericDataLoader.UNITS;
            testParams[IdleFantasyBackend.CHANGE] = "1";

            mBackend.MakeCloudCall( IdleFantasyBackend.TEST_CHANGE_TRAINING, testParams, null );

            yield return mBackend.WaitUntilNotBusy();
        }
    }
}

