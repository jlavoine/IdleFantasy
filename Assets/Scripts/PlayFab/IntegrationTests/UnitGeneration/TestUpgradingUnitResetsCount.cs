using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestUpgradingUnitResetsCount : TestUnitGeneration {
        private const long TIME_ELAPSED = 2000;
        private const float EXPECTED_COUNT = 0f;

        protected override IEnumerator RunAllTests() {
            yield return RunTest();
        }

        private IEnumerator RunTest() {
            yield return SetDataForTestPrep();
            yield return UpdateUnitCounts( TIME_ELAPSED );
            yield return UpgradeUnit();

            yield return FailTestIfUnitCountDoesNotEqual( EXPECTED_COUNT );
            yield return FailTestIfLastCountTimeNotUpdated();
        }

        private IEnumerator UpgradeUnit() {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[TARGET_ID] = UNIT_BEING_COUNTED;
            testParams[CLASS] = GenericDataLoader.UNITS;

            mBackend.MakeCloudCall( IdleFantasyBackend.TEST_UPGRADE, testParams, null );

            yield return mBackend.WaitUntilNotBusy();
        }
    }
}

