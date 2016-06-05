using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestIllegalElapsedTimeGeneratesNoUnits : TestUnitGeneration {
        private const long TIME_ELAPSED = -2000;
        private const float EXPECTED_COUNT = 0f;

        protected override IEnumerator RunAllTests() {
            yield return Test_IllegalElapsedTimeGeneratesNoUnits();
        }

        private IEnumerator Test_IllegalElapsedTimeGeneratesNoUnits() {
            SetPlayerData( SAVE_KEY_UNITS, SAVE_VALUE_UNITS );
            SetPlayerData( SAVE_KEY_BUILDINGS, SAVE_VALUE_BUILDINGS );
            yield return mBackend.WaitUntilNotBusy();

            yield return UpdateUnitCounts( TIME_ELAPSED );

            yield return FailTestIfUnitCountDoesNotEqual( EXPECTED_COUNT );
        }
    }
}
