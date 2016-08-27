using System.Collections;
using System.Collections.Generic;
using MyLibrary;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestAddMissingData : IntegrationTestBase {
        private List<string> DATA_KEYS_TO_TEST = new List<string>() { BackendConstants.BUILDING_PROGRESS, BackendConstants.UNIT_PROGRESS, BackendConstants.GUILD_PROGRESS, BackendConstants.WORLD_PROGRESS, BackendConstants.TRAINER_PROGRESS };     
        private const string EMPTY_SAVE_DATA = "{}";

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
                mBackend.MakeCloudCall( CloudTestMethods.getReadOnlyData.ToString(), cloudParams, ( results ) => {
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
            foreach ( string keyToTest in DATA_KEYS_TO_TEST ) {
                Dictionary<string, string> cloudParams = new Dictionary<string, string>() { { BackendConstants.SAVE_KEY, keyToTest } };
                mBackend.MakeCloudCall( CloudTestMethods.getReadOnlyData.ToString(), cloudParams, ( results ) => {
                    FailIfDataNotDefault( results[BackendConstants.DATA], keyToTest );
                } );
            }

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
            }
        }

        private void VerifyProgressIsDefault<T>( string i_saveData, string i_saveKey ) where T : ProgressBase {
            Dictionary<string, T> progressData = JsonConvert.DeserializeObject<Dictionary<string, T>>( i_saveData );
            foreach ( KeyValuePair<string, T> progress in progressData ) {
                if ( progress.Value.Level != 0 ) {
                    IntegrationTest.Fail( "Progress data level not default for " + i_saveKey + ": " + progress.Value.ID );
                }
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

        private void VerifyTrainerProgressDefault( string i_saveData ) {
            TrainerSaveData saveData = JsonConvert.DeserializeObject<TrainerSaveData>( i_saveData );
            foreach ( KeyValuePair<string, int> pair in saveData.TrainerCounts ) {
                if ( pair.Value != 0 ) {
                    IntegrationTest.Fail( "Trainer save data value not default: " + pair.Key );
                }
            }
        }
    }
}
