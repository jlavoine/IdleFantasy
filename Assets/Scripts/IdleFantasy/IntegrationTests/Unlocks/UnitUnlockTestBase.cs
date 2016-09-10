using MyLibrary;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class UnitUnlockTestBase : IntegrationTestBase {
        public const int STARTING_LEVEL = 0;

        public const string UNIT_TO_CHECK = "BASE_ARCHER_1";
        public const string BUILDING_TO_CHECK = "BASE_ARCHER_BUILDING_1";

        private const string START_UNIT_PROGRESS = "{\"BASE_ARCHER_1\":{\"Level\":0}}";
        private const string START_BUILDING_PROGRESS = "{\"BASE_ARCHER_BUILDING_1\":{\"Level\":0}}";

        protected abstract int GetTotalMissionsCompleted();
        protected abstract int GetExpectedLevel();

        protected override IEnumerator RunAllTests() {
            yield return SetPlayerDataOnServer();
            yield return RunUnlockCheck();

            yield return FailIfDataNotExpectedLevel( GenericDataLoader.UNITS, UNIT_TO_CHECK );
            yield return FailIfDataNotExpectedLevel( GenericDataLoader.BUILDINGS, BUILDING_TO_CHECK );
        }

        private IEnumerator SetPlayerDataOnServer() {
            SetUnitDataOnServer();
            SetBuildingDataOnServer();
            SetGameMetricData();

            yield return mBackend.WaitUntilNotBusy();
        }

        private void SetUnitDataOnServer() {
            IntegrationTestUtils.SetReadOnlyData( IntegrationTestUtils.SAVE_KEY_UNITS, START_UNIT_PROGRESS );
        }

        private void SetBuildingDataOnServer() {
            IntegrationTestUtils.SetReadOnlyData( IntegrationTestUtils.SAVE_KEY_BUILDINGS, START_BUILDING_PROGRESS );
        }

        private void SetGameMetricData() {
            GameMetrics metrics = new GameMetrics();
            metrics.Metrics = new Dictionary<string, int>();
            metrics.Metrics.Add( GameMetricsList.TOTAL_MISSIONS_DONE, GetTotalMissionsCompleted() );

            IntegrationTestUtils.SetReadOnlyData( IntegrationTestUtils.SAVE_KEY_METRICS, JsonConvert.SerializeObject( metrics ) );
        }

        private IEnumerator RunUnlockCheck() {
            yield return mBackend.WaitForCloudCall( CloudTestMethods.unitUnlockCheck.ToString(), PlayFabBackend.NULL_CLOUD_PARAMS, PlayFabBackend.NULL_CLOUD_CALLBACK );
        }

        private IEnumerator FailIfDataNotExpectedLevel( string i_class, string i_targetID ) {
            GetProgressData<UnitProgress>( i_class, i_targetID, ( result ) => {
                int expectedLevel = GetExpectedLevel();
                if ( result.Level != expectedLevel ) {
                    IntegrationTest.Fail( "Expecting level of " + i_targetID + " to be " + expectedLevel + " but was " + result.Level );
                }
            } );

            yield return mBackend.WaitUntilNotBusy();
        }
    }
}