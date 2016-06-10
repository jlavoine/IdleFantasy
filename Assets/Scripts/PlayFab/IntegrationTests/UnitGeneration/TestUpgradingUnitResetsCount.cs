using System.Collections;

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
            yield return IntegrationTestUtils.UpgradeTarget_NoRules( UNIT_BEING_COUNTED ,GenericDataLoader.UNITS );

            yield return FailTestIfUnitCountDoesNotEqual( EXPECTED_COUNT );
            yield return FailTestIfLastCountTimeNotUpdated();
        }
    }
}

