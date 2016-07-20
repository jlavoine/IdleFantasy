using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestUnitGenerationStopsAtMaxCapacity : TestUnitGeneration {
        private const long TIME_ELAPSED = long.MaxValue;

        private double mMaxCapacity;

        protected override IEnumerator RunAllTests() {
            yield return Test_UnitGenerationStopsAtMaxCapacity();
        }

        private IEnumerator Test_UnitGenerationStopsAtMaxCapacity() {
            yield return SetDataForTestPrep();
            yield return SetMaxCapacity();

            yield return UpdateUnitCounts( TIME_ELAPSED );

            yield return FailTestIfUnitCountDoesNotEqual( (float)mMaxCapacity );
        }

        private IEnumerator SetMaxCapacity() {
            yield return GetNumberFromCloudCall( CloudTestMethods.getCapacityForUnit.ToString(),
                new Dictionary<string, string>() { { BackendConstants.TARGET_ID, UNIT_BEING_COUNTED } },
                (result) => {
                    mMaxCapacity = result;
                } );
        }
    }
}
