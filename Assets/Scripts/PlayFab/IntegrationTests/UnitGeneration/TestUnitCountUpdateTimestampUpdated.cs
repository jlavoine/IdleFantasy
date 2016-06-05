using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestUnitCountUpdateTimestampUpdated : TestUnitGeneration {
        private const long TIME_ELAPSED = 2000;

        protected override IEnumerator RunAllTests() {
            yield return Test_UnitCountUpdateTimestampUpdated();
        }

        private IEnumerator Test_UnitCountUpdateTimestampUpdated() {
            SetPlayerData( SAVE_KEY_UNITS, SAVE_VALUE_UNITS );
            SetPlayerData( SAVE_KEY_BUILDINGS, SAVE_VALUE_BUILDINGS );
            SetInternalData( "LastUnitCountTime", "0" );
            yield return mBackend.WaitUntilNotBusy();

            yield return UpdateUnitCounts( TIME_ELAPSED );

            yield return FailTestIfLastUpdateTimestampNotUpdated( 0 );
        }
    }
}
