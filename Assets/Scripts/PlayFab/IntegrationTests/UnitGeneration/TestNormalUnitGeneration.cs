using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestNormalUnitGeneration : TestUnitGeneration {
        private const long TIME_ELAPSED = 2000;
        private const float EXPECTED_COUNT = 1f;

        protected override IEnumerator RunAllTests() {
            yield return Test_NormalUnitGeneration();
        }

        private IEnumerator Test_NormalUnitGeneration() {
            SetPlayerData( SAVE_KEY_UNITS, SAVE_VALUE_UNITS );
            SetPlayerData( SAVE_KEY_BUILDINGS, SAVE_VALUE_BUILDINGS );
            yield return mBackend.WaitUntilNotBusy();

            yield return UpdateUnitCounts( TIME_ELAPSED );

            yield return FailTestIfUnitCountDoesNotEqual( EXPECTED_COUNT );
        }
    }
}
