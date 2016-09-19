using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class TravelToSelectionTestBase : IntegrationTestBase {
        private const int TEST_LEVEL = 1;
        private const string TEST_WORLD = "Base";
        protected const int TEST_SIZE = 36;

        protected abstract IEnumerator RunOtherFailureChecks();
        protected abstract Dictionary<string,WorldMissionProgress> GetMissionProgressForPlayer();

        private MapData mMapData;
        public MapData MapData { get { return mMapData; } }

        protected override IEnumerator RunAllTests() {
            yield return SetPlayerDataOnServer();            

            yield return SendTravelRequestToServer();

            if ( IsTestExpectedToFail() ) {
                FailTestIfClientInSync( GetType().ToString() );
            } else {
                FailTestIfClientOutOfSync( GetType().ToString() );
            }

            yield return RunOtherFailureChecks();
        }

        private IEnumerator SetPlayerDataOnServer() {
            SetMapSaveData();            
            yield return mBackend.WaitUntilNotBusy();

            // do this AFTER setting map data because that test method RESETS progress
            SetMissionProgressSaveData();
            yield return mBackend.WaitUntilNotBusy();
        }        

        private void SetMissionProgressSaveData() {
            Dictionary<string, WorldMissionProgress> progress = GetMissionProgressForPlayer();
            IntegrationTestUtils.SetReadOnlyData( BackendConstants.MISSION_PROGRESS, JsonConvert.SerializeObject( progress ) );
        }

        private IEnumerator SendTravelRequestToServer() {
            Dictionary<string, string> cloudParams = new Dictionary<string, string>();
            cloudParams.Add( BackendConstants.MAP_WORLD, BackendConstants.WORLD_BASE );
            cloudParams.Add( BackendConstants.INDEX, 0.ToString() );

            mBackend.MakeCloudCall( BackendConstants.TRAVEL_TO, cloudParams, null );

            yield return mBackend.WaitUntilNotBusy();
        }

        private void SetMapSaveData() {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[BackendConstants.MAP_LEVEL] = TEST_LEVEL.ToString();
            testParams[BackendConstants.MAP_WORLD] = TEST_WORLD;
            testParams[BackendConstants.MAP_SIZE] = TEST_SIZE.ToString();

            mBackend.MakeCloudCall( CloudTestMethods.createMapForTesting.ToString(), testParams, ( results ) => {
                mMapData = JsonConvert.DeserializeObject<MapData>( results[BackendConstants.DATA] );
                IntegrationTestUtils.SetReadOnlyData( BackendConstants.MAP_BASE, JsonConvert.SerializeObject( mMapData ) );
            } );
        }
    }
}
