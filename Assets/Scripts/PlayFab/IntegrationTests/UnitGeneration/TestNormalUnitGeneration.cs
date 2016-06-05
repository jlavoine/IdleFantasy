using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestNormalUnitGeneration : TestUnitGeneration {
        private const long TIME_ELAPSED = 2000;
        private const float EXPECTED_COUNT = 1f;

        protected override IEnumerator RunAllTests() {
            yield return Test_NormalUnitGeneration();
        }

        private IEnumerator Test_NormalUnitGeneration() {
            yield return SetDataForTestPrep();

            yield return UpdateUnitCounts( TIME_ELAPSED );

            yield return FailTestIfUnitCountDoesNotEqual( EXPECTED_COUNT );
        }
    }
}
