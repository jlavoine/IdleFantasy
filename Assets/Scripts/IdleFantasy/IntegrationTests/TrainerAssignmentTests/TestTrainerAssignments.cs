using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class TestTrainerAssignments : IntegrationTestBase {
        protected const string TRAINER_DATA_KEY = "TrainerSaveData";
        protected const string TRAINER_DATA = "{\"TrainerCounts\":{\"Normal\":$NUM$}}";
        protected const string PROGRESS_KEY = "UnitsProgress";
        protected const string PROGRESS_DATA = "{\"BASE_WARRIOR_1\":{\"Level\":$LEVEL$,\"Trainers\":$TRAINERS$}}";
        protected const string UNIT_ID = "BASE_WARRIOR_1";        

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

            IntegrationTestUtils.SetReadOnlyData( PROGRESS_KEY, data );
        }

        protected IEnumerator MakeAssignmentChange( int i_change ) {
            mBackend.ChangeAssignedTrainers( UNIT_ID, i_change );
            yield return mBackend.WaitUntilNotBusy();
        }
    }
}