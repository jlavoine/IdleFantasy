using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestUnitGenerationStopsAtMaxCapacity : TestUnitGeneration {
        private const long TIME_ELAPSED = 1000000;
        private const float EXPECTED_COUNT = 300f;

        protected override IEnumerator RunAllTests() {
            yield return Test_UnitGenerationStopsAtMaxCapacity();
        }

        private IEnumerator Test_UnitGenerationStopsAtMaxCapacity() {
            yield return SetDataForTestPrep();

            yield return UpdateUnitCounts( TIME_ELAPSED );

            yield return FailTestIfUnitCountDoesNotEqual( EXPECTED_COUNT );
        }
    }
}
