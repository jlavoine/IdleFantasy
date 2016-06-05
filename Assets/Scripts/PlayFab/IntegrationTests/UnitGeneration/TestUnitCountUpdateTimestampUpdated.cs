using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestUnitCountUpdateTimestampUpdated : TestUnitGeneration {
        private const long TIME_ELAPSED = 2000;

        protected override IEnumerator RunAllTests() {
            yield return Test_UnitCountUpdateTimestampUpdated();
        }

        private IEnumerator Test_UnitCountUpdateTimestampUpdated() {
            yield return SetDataForTestPrep();

            yield return UpdateUnitCounts( TIME_ELAPSED );

            yield return FailTestIfLastCountTimeNotUpdated( 0 );
        }
    }
}
