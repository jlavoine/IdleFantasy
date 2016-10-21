using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class TestTrainerAssignments : IntegrationTestBase {
        protected const string TRAINER_DATA_KEY = "TrainerSaveData";
        protected const string TRAINER_DATA = "{\"TrainerCounts\":{\"Normal\":$NUM$}}";
        protected const string PROGRESS_KEY = "UnitsProgress";
        protected const string PROGRESS_DATA = "{\"BASE_WARRIOR_1\":{\"Level\":$LEVEL$,\"Trainers\":$TRAINERS$, \"ClientTimestamp\":$TIME$}}";
        protected const string UNIT_ID = "BASE_WARRIOR_1";

        protected const int LOGGED_IN_TIME = 0;
        protected const int CURRENT_CLIENT_TIMESTAMP = 100;
        protected const int CLIENT_TIMESTAMP = 1000;

        protected abstract IEnumerator RunTrainingTests();

        protected override IEnumerator RunAllTests() {
            SetBuildingProgressData();
            yield return mBackend.WaitUntilNotBusy();
            yield return IntegrationTestUtils.SetInternalData( BackendConstants.LOGGED_IN_TIME, LOGGED_IN_TIME.ToString() );
            yield return RunTrainingTests();
        }

        private void SetBuildingProgressData() {
            string progressData = "{\"BASE_WARRIOR_BUILDING_1\":{\"Level\":100}}";
            IntegrationTestUtils.SetReadOnlyData( IntegrationTestUtils.SAVE_KEY_BUILDINGS, progressData );
        }

        protected void FailTestIfAssignedTrainersDoesNotEqual( int i_trainers ) {
            Dictionary<string, string> getParams = new Dictionary<string, string>();
            getParams.Add( "Class", "Units" );
            getParams.Add( "TargetID", UNIT_ID );

            mBackend.MakeCloudCall( CloudTestMethods.getProgressData.ToString(), getParams, ( results ) => {
                if ( results.ContainsKey( "data" ) ) {
                    UnitProgress progress = JsonConvert.DeserializeObject<UnitProgress>( results[BackendConstants.DATA] );
                    if ( progress.Trainers != i_trainers ) {
                        IntegrationTest.Fail( "Trainers did not match: " + i_trainers );
                    }
                }
                else {
                    IntegrationTest.Fail( "Results did not have data." );
                }
            } );
        }

        protected void SetTrainerCount( int i_trainers ) {
            IntegrationTestUtils.SetReadOnlyData( TRAINER_DATA_KEY, DrsStringUtils.Replace( TRAINER_DATA, "NUM", i_trainers ) );
        }

        protected void SetProgressData( int i_level, int i_trainers ) {
            string data = PROGRESS_DATA;
            data = DrsStringUtils.Replace( data, "LEVEL", i_level );
            data = DrsStringUtils.Replace( data, "TRAINERS", i_trainers );
            data = DrsStringUtils.Replace( data, "TIME", CURRENT_CLIENT_TIMESTAMP );

            IntegrationTestUtils.SetReadOnlyData( PROGRESS_KEY, data );
        }

        protected IEnumerator MakeAssignmentChange( int i_change ) {
            ChangeAssignedTrainers( UNIT_ID, i_change, GetClientTimestampForChange() );
            yield return mBackend.WaitUntilNotBusy();
        }

        protected virtual int GetClientTimestampForChange() {
            return CLIENT_TIMESTAMP;
        }

        private void ChangeAssignedTrainers( string i_unitID, int i_change, int i_clientTimestamp ) {
            Dictionary<string, string> assignParams = new Dictionary<string, string>();
            assignParams.Add( BackendConstants.CHANGE, i_change.ToString() );
            assignParams.Add( BackendConstants.TARGET_ID, i_unitID );
            assignParams.Add( BackendConstants.CLIENT_TIMESTAMP, i_clientTimestamp.ToString() );

            mBackend.MakeCloudCall( BackendConstants.INIT_TRAINING_CHANGE, assignParams, null );
        }
    }
}