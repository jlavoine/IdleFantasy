using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class TestUnitGeneration : IntegrationTestBase {
        private const string TIME_KEY = "Time";

        protected const string LAST_UNIT_COUNT_TIME = "LastCountTime";                
        protected const string SAVE_KEY_BUILDINGS = "BuildingsProgress";
        protected const string SAVE_VALUE_UNITS = "{\"BASE_MELEE_1\":{\"Level\":1, \"Count\":0, \"Trainers\":0, \"LastCountTime\":0}}";        
        protected const string SAVE_VALUE_BUILDINGS = "{\"BASE_BUILDING_1\":{\"Level\":1}}";
        protected const string UNIT_BEING_COUNTED = "BASE_MELEE_1";

        protected IEnumerator SetDataForTestPrep() {
            IntegrationTestUtils.SetReadOnlyData( IntegrationTestUtils.SAVE_KEY_UNITS, SAVE_VALUE_UNITS );
            IntegrationTestUtils.SetReadOnlyData( SAVE_KEY_BUILDINGS, SAVE_VALUE_BUILDINGS );
                        
            yield return mBackend.WaitUntilNotBusy();
        }

        protected IEnumerator UpdateUnitCounts( long i_elapsedTime ) {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[TIME_KEY] = i_elapsedTime.ToString();

            mBackend.MakeCloudCall( CloudTestMethods.testUpdateUnitCount.ToString(), testParams, null );
            yield return mBackend.WaitUntilNotBusy();
        }

        protected IEnumerator FailTestIfUnitCountDoesNotEqual( float i_count ) {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[BackendConstants.TARGET_ID] = UNIT_BEING_COUNTED;

            FailTestIfReturnedCallDoesNotEqual( CloudTestMethods.getUnitCount.ToString(), i_count, testParams );

            yield return mBackend.WaitUntilNotBusy();
        }

        protected IEnumerator FailTestIfLastCountTimeDoesNotEqual( double i_time ) {
            GetProgressData<UnitProgress>( GenericDataLoader.UNITS, UNIT_BEING_COUNTED, ( result ) => {
                if ( result.LastCountTime != i_time ) {
                    IntegrationTest.Fail( "Expecting last count time to be " + i_time + " but was " + result.LastCountTime );
                }
            } );

            yield return mBackend.WaitUntilNotBusy();
        }

        protected IEnumerator FailTestIfLastCountTimeNotUpdated() {
            GetProgressData<UnitProgress>( GenericDataLoader.UNITS, UNIT_BEING_COUNTED, ( result ) => {
                if ( result.LastCountTime == 0 ) {
                    IntegrationTest.Fail( "Expecting last count time to NOT be 0!" );
                }
            } );

            yield return mBackend.WaitUntilNotBusy();
        }
    }
}
