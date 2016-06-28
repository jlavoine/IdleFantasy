using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestIllegalElapsedTimeGeneratesNoUnits : TestUnitGeneration {
        private const long TIME_ELAPSED = -2000;
        private const float EXPECTED_COUNT = 0f;

        protected override IEnumerator RunAllTests() {
            yield return Test_IllegalElapsedTimeGeneratesNoUnits();
        }

        private IEnumerator Test_IllegalElapsedTimeGeneratesNoUnits() {
            yield return SetDataForTestPrep();

            yield return UpdateUnitCounts( TIME_ELAPSED );

            yield return FailTestIfUnitCountDoesNotEqual( EXPECTED_COUNT );
        }
    }
}
