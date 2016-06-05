using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class TestUnitGeneration : IntegrationTestBase {
        private const string TIME_ELAPSED_KEY = "TimeElapsed";

        protected const string LAST_UNIT_COUNT_TIME = "LastUnitCountTime";

        protected const string GET_UNIT_COUNT_METHOD = "getUnitCount";

        protected const string SAVE_KEY_UNITS = "UnitsProgress";
        protected const string SAVE_KEY_BUILDINGS = "BuildingsProgress";
        protected const string SAVE_VALUE_UNITS = "{\"BASE_MELEE_1\":{\"Level\":1, \"Count\":0, \"Trainers\":0}}";        
        protected const string SAVE_VALUE_BUILDINGS = "{\"BASE_BUILDING_1\":{\"Level\":1}}";
        protected const string UNIT_BEING_COUNTED = "BASE_MELEE_1";

        protected IEnumerator SetDataForTestPrep() {
            SetPlayerData( SAVE_KEY_UNITS, SAVE_VALUE_UNITS );
            SetPlayerData( SAVE_KEY_BUILDINGS, SAVE_VALUE_BUILDINGS );
            SetInternalData( LAST_UNIT_COUNT_TIME, "0" );               // reset the last count time so the tests know it was updated
            yield return mBackend.WaitUntilNotBusy();
        }

        protected IEnumerator UpdateUnitCounts( long i_elapsedTime ) {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[TIME_ELAPSED_KEY] = i_elapsedTime.ToString();

            mBackend.MakeCloudCall( IdleFantasyBackend.TEST_UPDATE_UNIT_COUNT, testParams, null );
            yield return mBackend.WaitUntilNotBusy();
        }

        protected IEnumerator FailTestIfUnitCountDoesNotEqual( float i_count ) {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[TARGET_ID] = UNIT_BEING_COUNTED;

            FailTestIfReturnedCallDoesNotEqual( GET_UNIT_COUNT_METHOD, i_count, testParams );

            yield return mBackend.WaitUntilNotBusy();
        }

        protected IEnumerator FailTestIfLastCountTimeNotUpdated( double i_time ) {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[SAVE_KEY] = LAST_UNIT_COUNT_TIME;

            FailTestIfReturnedCallEquals( IdleFantasyBackend.TEST_GET_INTERNAL_DATA, i_time, testParams );

            yield return mBackend.WaitUntilNotBusy();
        }
    }
}
