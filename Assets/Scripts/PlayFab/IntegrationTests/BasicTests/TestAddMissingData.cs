using System.Collections;
using System.Collections.Generic;
using MyLibrary;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestAddMissingData : IntegrationTestBase {
        private List<string> DATA_KEYS_TO_TEST = new List<string>() { BackendConstants.MAP_BASE, BackendConstants.BUILDING_PROGRESS, BackendConstants.UNIT_PROGRESS, BackendConstants.GUILD_PROGRESS, BackendConstants.WORLD_PROGRESS, BackendConstants.TRAINER_PROGRESS, BackendConstants.MISSION_PROGRESS, BackendConstants.GAME_METRICS };        
        private const string EMPTY_SAVE_DATA = "{}";
        private const int DEFAULT_MAP_SIZE = 36;
        private const int STARTING_GOLD = 1000;

        private const string STARTING_BUILDING = "BASE_WARRIOR_BUILDING_1";
        private const string STARTING_UNIT = "BASE_WARRIOR_1";

        protected override IEnumerator RunAllTests() {
            yield return DeleteAllPlayerSaveData();
            yield return VerifyPlayerSaveDataIsEmpty();
            yield return AddMissingSaveData();
            yield return VerifyAllSaveData();
        }

        private IEnumerator DeleteAllPlayerSaveData() {            
            yield return mBackend.WaitForCloudCall( CloudTestMethods.deleteAllPlayerReadOnlyData.ToString(), PlayFabBackend.NULL_CLOUD_PARAMS, PlayFabBackend.NULL_CLOUD_CALLBACK );
        }

        private IEnumerator VerifyPlayerSaveDataIsEmpty() {
            foreach ( string keyToTest in DATA_KEYS_TO_TEST ) {
                Dictionary<string, string> cloudParams = new Dictionary<string, string>() { { BackendConstants.SAVE_KEY, keyToTest } };
                yield return mBackend.WaitForCloudCall( CloudTestMethods.getReadOnlyData.ToString(), cloudParams, ( results ) => {
                    FailIfSaveDataNotEmpty( results[BackendConstants.DATA], keyToTest );                    
                } );
            }

            yield return mBackend.WaitUntilNotBusy();
        }

        private void FailIfSaveDataNotEmpty( string i_saveData, string i_saveKey ) {
            if ( i_saveData != EMPTY_SAVE_DATA ) {
                IntegrationTest.Fail( "Save data should be empty but it wasn't: " + i_saveKey );
            }
        }

        private IEnumerator AddMissingSaveData() {
            yield return mBackend.WaitForCloudCall( CloudTestMethods.addMissingPlayerData.ToString(), PlayFabBackend.NULL_CLOUD_PARAMS, PlayFabBackend.NULL_CLOUD_CALLBACK );
        }

        private IEnumerator VerifyAllSaveData() {
            yield return FailIfStartingCurrencyNotApplied();

            foreach ( string keyToTest in DATA_KEYS_TO_TEST ) {
                Dictionary<string, string> cloudParams = new Dictionary<string, string>() { { BackendConstants.SAVE_KEY, keyToTest } };
                yield return mBackend.WaitForCloudCall( CloudTestMethods.getReadOnlyData.ToString(), cloudParams, ( results ) => {
                    FailIfDataNotDefault( results[BackendConstants.DATA], keyToTest );
                } );
            }

            yield return mBackend.WaitUntilNotBusy();
        }

        private IEnumerator FailIfStartingCurrencyNotApplied() {
            Dictionary<string, string> cloudParams = new Dictionary<string, string>() { { BackendConstants.TYPE, VirtualCurrencies.GOLD } };
            FailTestIfReturnedCallDoesNotEqual( CloudTestMethods.getPlayerCurrency.ToString(), STARTING_GOLD, cloudParams );

            yield return mBackend.WaitUntilNotBusy();
        }

        // This method is messy...
        private void FailIfDataNotDefault( string i_saveData, string i_saveKey ) {
            switch ( i_saveKey ) {
                case BackendConstants.BUILDING_PROGRESS:
                    VerifyProgressIsDefault<BuildingProgress>( i_saveData, i_saveKey );
                    break;
                case BackendConstants.GUILD_PROGRESS:
                    VerifyProgressIsDefault<GuildProgress>( i_saveData, i_saveKey );
                    break;
                case BackendConstants.UNIT_PROGRESS:
                    VerifyProgressIsDefault<UnitProgress>( i_saveData, i_saveKey );
                    break;
                case BackendConstants.WORLD_PROGRESS:
                    VerifyWorldProgressIsDefault( i_saveData );
                    break;
                case BackendConstants.TRAINER_PROGRESS:
                    VerifyTrainerProgressDefault( i_saveData );
                    break;
                case BackendConstants.MAP_BASE:
                    VerifyFirstMapIsDefault( i_saveData );
                    break;
                case BackendConstants.MISSION_PROGRESS:
                    VerifyMissionProgressIsDefault( i_saveData );
                    break;
                case BackendConstants.GAME_METRICS:
                    VerifyGameMetricsIsDefault( i_saveData );
                    break;
                default:
                    UnityEngine.Debug.LogError( "No case for default data check on " + i_saveKey );
                    break;
            }
        }

        private void VerifyProgressIsDefault<T>( string i_saveData, string i_saveKey ) where T : ProgressBase {
            Dictionary<string, T> progressData = JsonConvert.DeserializeObject<Dictionary<string, T>>( i_saveData );
            foreach ( KeyValuePair<string, T> progress in progressData ) {
                int defaultValue = GetDefaultValueForProgress( progress.Value.ID );
                if ( progress.Value.Level != defaultValue ) {
                    IntegrationTest.Fail( "Progress data level not default for " + i_saveKey + ": " + progress.Value.ID );
                }
            }
        }

        private int GetDefaultValueForProgress(string i_ID ) {
            if ( i_ID == STARTING_BUILDING || i_ID == STARTING_UNIT ) {
                return 1;
            } else {
                return 0;
            }
        }

        private void VerifyWorldProgressIsDefault( string i_saveData ) {
            Dictionary<string, WorldProgress> worldProgress = JsonConvert.DeserializeObject<Dictionary<string, WorldProgress>>( i_saveData );
            foreach ( KeyValuePair<string, WorldProgress> pair in worldProgress ) {
                WorldProgress world = pair.Value;
                if ( world.CurrentMapLevel != 0 || world.RestartCount != 0 ) {
                    IntegrationTest.Fail( "World progress for " + world.ID + " not default" );
                }
            }
        }

        private void VerifyGameMetricsIsDefault( string i_saveData ) {
            GameMetrics metrics = JsonConvert.DeserializeObject<GameMetrics>( i_saveData );
            if ( metrics.Metrics.Count != 0 ) {
                IntegrationTest.Fail( "Game metrics was not empty" );
            }
        }

        private void VerifyTrainerProgressDefault( string i_saveData ) {
            TrainerSaveData saveData = JsonConvert.DeserializeObject<TrainerSaveData>( i_saveData );
            foreach ( KeyValuePair<string, int> pair in saveData.TrainerCounts ) {
                if ( pair.Value != 0 ) {
                    IntegrationTest.Fail( "Trainer save data value not default: " + pair.Key );
                }
            }
        }

        private void VerifyFirstMapIsDefault( string i_saveData ) {
            Dictionary<string, string> cloudParams = new Dictionary<string, string>() { { BackendConstants.SAVE_KEY, BackendConstants.MAP_BASE } };
            mBackend.MakeCloudCall( CloudTestMethods.getReadOnlyData.ToString(), cloudParams, ( results ) => {
                if ( results[BackendConstants.DATA] != i_saveData ) {
                    IntegrationTest.Fail( "First map default data did not match title data" );
                }
            } );
        }

        private void VerifyMissionProgressIsDefault( string i_saveData ) {
            Dictionary<string, string> cloudParams = new Dictionary<string, string>() { { BackendConstants.SAVE_KEY, BackendConstants.MISSION_PROGRESS } };
            mBackend.MakeCloudCall( CloudTestMethods.getReadOnlyData.ToString(), cloudParams, ( results ) => {
                Dictionary<string, WorldMissionProgress> allMissionProgress = JsonConvert.DeserializeObject<Dictionary<string, WorldMissionProgress>>( results[BackendConstants.DATA] );
                WorldMissionProgress baseWorldMissionProgress = allMissionProgress[BackendConstants.WORLD_BASE];

                if ( baseWorldMissionProgress.Missions.Count != DEFAULT_MAP_SIZE ) {
                    IntegrationTest.Fail( "Missions completed list was not default: " + baseWorldMissionProgress.Missions.Count );
                }

                foreach (SingleMissionProgress singleMission in baseWorldMissionProgress.Missions ) {
                    if ( singleMission.Completed != false ) {
                        IntegrationTest.Fail( "A mission was marked as completed when it should not have been." );
                    }
                }
            } );
        }
    }
}
