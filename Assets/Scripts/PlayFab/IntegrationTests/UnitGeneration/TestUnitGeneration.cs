using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class TestUnitGeneration : IntegrationTestBase {
        private const string TIME_ELAPSED_KEY = "TimeElapsed";

        protected const string GET_UNIT_COUNT = "getUnitCount";

        protected const string SAVE_KEY_UNITS = "UnitsProgress";
        protected const string SAVE_VALUE_UNITS = "{\"BASE_MELEE_1\":{\"Level\":1, \"Count\":0, \"Trainers\":0}}";
        protected const string SAVE_KEY_BUILDINGS = "BuildingsProgress";
        protected const string SAVE_VALUE_BUILDINGS = "{\"BASE_BUILDING_1\":{\"Level\":1}}";

        protected IEnumerator UpdateUnitCounts( long i_elapsedTime ) {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[TIME_ELAPSED_KEY] = i_elapsedTime.ToString();

            mBackend.MakeCloudCall( IdleFantasyBackend.TEST_UPDATE_UNIT_COUNT, testParams, null );
            yield return mBackend.WaitUntilNotBusy();
        }

        protected IEnumerator FailTestIfUnitCountDoesNotEqual( float i_count ) {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[TARGET_ID] = "BASE_MELEE_1";

            FailTestIfReturnedCallDoesNotEqual( GET_UNIT_COUNT, i_count, testParams );

            yield return mBackend.WaitUntilNotBusy();
        }

        protected IEnumerator FailTestIfLastUpdateTimestampNotUpdated( double i_time ) {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams["SaveKey"] = "LastUnitCountTime";

            FailTestIfReturnedCallEquals( IdleFantasyBackend.TEST_GET_INTERNAL_DATA, i_time, testParams );

            yield return mBackend.WaitUntilNotBusy();
        }
    }
}
