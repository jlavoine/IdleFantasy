using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class PlayerDataResetsWhenWorldResets : WorldResetTestsBase {
        private List<string> DATA_KEYS_TO_TEST = new List<string>() { BackendConstants.MAP_BASE, BackendConstants.BUILDING_PROGRESS, BackendConstants.UNIT_PROGRESS, BackendConstants.TRAINER_PROGRESS, BackendConstants.MISSION_PROGRESS, BackendConstants.WORLD_PROGRESS };

        protected override int GetMapLevel() {
            return 10;
        }

        protected override IEnumerator RunOtherFailureChecks() {
            yield return VerifySaveDataIsReset();
        }

        protected IEnumerator VerifySaveDataIsReset() {
            yield return FailIfStartingCurrencyNotApplied();

            foreach ( string keyToTest in DATA_KEYS_TO_TEST ) {
                Dictionary<string, string> cloudParams = new Dictionary<string, string>() { { BackendConstants.SAVE_KEY, keyToTest } };
                yield return mBackend.WaitForCloudCall( CloudTestMethods.getReadOnlyData.ToString(), cloudParams, ( results ) => {
                    FailIfDataNotReset( results[BackendConstants.DATA], keyToTest );
                } );
            }

            yield return mBackend.WaitUntilNotBusy();
        }

        private void FailIfDataNotReset( string i_saveData, string i_saveKey ) {
            switch ( i_saveKey ) {
                case BackendConstants.BUILDING_PROGRESS:
                    VerifyProgressIsReset<BuildingProgress>( i_saveData, i_saveKey );
                    break;
                case BackendConstants.UNIT_PROGRESS:
                    VerifyProgressIsReset<UnitProgress>( i_saveData, i_saveKey );
                    break;
                case BackendConstants.WORLD_PROGRESS:
                    VerifyWorldProgressIncremented( i_saveData );
                    break;
                case BackendConstants.TRAINER_PROGRESS:
                    VerifyTrainerProgressReset( i_saveData );
                    break;
                case BackendConstants.MAP_BASE:
                    VerifyMapIsLevelOne( i_saveData );
                    break;
                case BackendConstants.MISSION_PROGRESS:
                    VerifyMissionProgressIsReset( i_saveData );
                    break;
                default:
                    UnityEngine.Debug.LogError( "No case for reset data check on " + i_saveKey );
                    break;
            }
        }

        private void VerifyProgressIsReset<T>( string i_saveData, string i_saveKey ) where T : ProgressBase {
            Dictionary<string, T> progressData = JsonConvert.DeserializeObject<Dictionary<string, T>>( i_saveData );
            foreach ( KeyValuePair<string, T> progress in progressData ) {
                if ( progress.Value.Level != 1 ) {
                    IntegrationTest.Fail( "Progress data level not reset for " + i_saveKey + ": " + progress.Value.ID );
                }
            }
        }

        private void VerifyWorldProgressIncremented( string i_saveData ) {
            Dictionary<string, WorldProgress> worldProgress = JsonConvert.DeserializeObject<Dictionary<string, WorldProgress>>( i_saveData );
            foreach ( KeyValuePair<string, WorldProgress> pair in worldProgress ) {
                if ( pair.Key == TEST_WORLD && pair.Value.RestartCount != 1 ) {
                    IntegrationTest.Fail( "World restart count did not increment after reset." );
                }
            }
        }

        private void VerifyTrainerProgressReset( string i_saveData ) {
            TrainerSaveData saveData = JsonConvert.DeserializeObject<TrainerSaveData>( i_saveData );
            foreach ( KeyValuePair<string, int> pair in saveData.TrainerCounts ) {
                if ( pair.Key == TrainerManager.NORMAL_TRAINERS && pair.Value != 0 ) {
                    IntegrationTest.Fail( "Trainer save data value not reset: " + pair.Key );
                }
            }
        }

        private void VerifyMapIsLevelOne( string i_saveData ) {
            MapData map = JsonConvert.DeserializeObject<MapData>( i_saveData );
            if ( map.MapLevel != 1 ) {
                IntegrationTest.Fail( "Map was not level 1 after a world reset." );
            }
        }

        private void VerifyMissionProgressIsReset( string i_saveData ) {
            Dictionary<string, WorldMissionProgress> allMissionProgress = JsonConvert.DeserializeObject<Dictionary<string, WorldMissionProgress>>( i_saveData );
            WorldMissionProgress baseWorldMissionProgress = allMissionProgress[BackendConstants.WORLD_BASE];

            foreach ( SingleMissionProgress singleMission in baseWorldMissionProgress.Missions ) {
                if ( singleMission.Completed != false ) {
                    IntegrationTest.Fail( "A mission was marked as completed when it should not have been." );
                }
            }
        }

        private IEnumerator FailIfStartingCurrencyNotApplied() {
            Dictionary<string, string> cloudParams = new Dictionary<string, string>() { { BackendConstants.TYPE, VirtualCurrencies.GOLD } };
            FailTestIfReturnedCallDoesNotEqual( CloudTestMethods.getPlayerCurrency.ToString(), IntegrationTestUtils.STARTING_GOLD, cloudParams );

            yield return mBackend.WaitUntilNotBusy();
        }
    }
}