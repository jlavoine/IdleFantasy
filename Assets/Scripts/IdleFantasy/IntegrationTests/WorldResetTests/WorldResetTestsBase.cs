using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class WorldResetTestsBase : IntegrationTestBase {
        public const string TEST_WORLD = "Base";
        public const int TEST_SIZE = 25;

        protected const string TRAINER_SAVE_DATA = "{\"TrainerCounts\":{\"Normal\":5}}";
        protected const string UNIT_SAVE_DATA = "{\"BASE_WARRIOR_1\":{\"Level\":3,\"Trainers\":1}}";
        protected const string BUILDING_SAVE_DATA = "{\"BASE_BUILDING_1\":{\"Level\":10}}";
        protected const string WORLD_PROGRESS_SAVE_DATA = "{\"Base\":{\"ID\":\"Base\",\"RestartCount\":0,\"CurrentMapLevel\":0}}";

        protected abstract int GetMapLevel();
        protected abstract IEnumerator RunOtherFailureChecks();

        protected override IEnumerator RunAllTests() {
            yield return SetPlayerDataOnServer();

            yield return SendWorldResetRequestToServer();

            if ( IsTestExpectedToFail() ) {
                FailTestIfClientInSync( GetType().ToString() );
            }
            else {
                FailTestIfClientOutOfSync( GetType().ToString() );
            }

            yield return RunOtherFailureChecks();
        }

        private IEnumerator SetPlayerDataOnServer() {
            IntegrationTestUtils.CreateAndSetRandomMapSaveData( TEST_WORLD, GetMapLevel(), TEST_SIZE, null );
            yield return mBackend.WaitUntilNotBusy();

            // do this AFTER setting map data because that test method RESETS progress
            SetProgressData();
            yield return mBackend.WaitUntilNotBusy();
        }

        private void SetProgressData() {
            IntegrationTestUtils.SetReadOnlyData( IntegrationTestUtils.TRAINER_DATA_KEY, TRAINER_SAVE_DATA );
            IntegrationTestUtils.SetReadOnlyData( IntegrationTestUtils.SAVE_KEY_BUILDINGS, BUILDING_SAVE_DATA );
            IntegrationTestUtils.SetReadOnlyData( IntegrationTestUtils.SAVE_KEY_UNITS, UNIT_SAVE_DATA );
            IntegrationTestUtils.SetReadOnlyData( BackendConstants.WORLD_PROGRESS, WORLD_PROGRESS_SAVE_DATA );
        }

        private IEnumerator SendWorldResetRequestToServer() {
            Dictionary<string, string> cloudParams = new Dictionary<string, string>();
            cloudParams.Add( BackendConstants.MAP_WORLD, BackendConstants.WORLD_BASE );

            mBackend.MakeCloudCall( BackendConstants.RESET_WORLD_REQUEST, cloudParams, null );

            yield return mBackend.WaitUntilNotBusy();
        }
    }
}
