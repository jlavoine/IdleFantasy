using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestTrainerAssignments : IntegrationTestBase {
        private const string TRAINER_DATA_KEY = "TrainerSaveData";
        private const string TRAINER_DATA = "{\"TrainerCounts\":{\"Normal\":$NUM$}}";
        private const string PROGRESS_KEY = "UnitsProgress";
        private const string PROGRESS_DATA = "{\"BASE_MELEE_1\":{\"Level\":$LEVEL$,\"Trainers\":$TRAINERS$}}";
        private const string UNIT_ID = "BASE_MELEE_1";

        protected override IEnumerator RunAllTests() {
            yield return mBackend.WaitUntilNotBusy();

            yield return Test_LegalAssignment();
            yield return Test_CannotTrainWithNoTrainers();
            yield return Test_CannotTrainWhenUnitAlreadyTrainedToMax();
            yield return Test_CannotUntrainWithZeroAssignedTrainers();

            DoneWithTests();
        }

        private IEnumerator Test_LegalAssignment() {
            SetTrainerCount( 1 );
            yield return mBackend.WaitUntilNotBusy();
            SetProgressData( 1, 0 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeAssignmentChange( 1 );

            FailTestIfAvailableTrainersDoesNotEqual( 0 );
            FailTestIfAssignedTrainersDoesNotEqual( 1 );

            yield return mBackend.WaitUntilNotBusy();
        }

        private IEnumerator Test_CannotTrainWithNoTrainers() {
            SetTrainerCount( 0 );
            yield return mBackend.WaitUntilNotBusy();
            SetProgressData( 1, 0 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeAssignmentChange( 1 );

            FailTestIfClientInSync( "Test_CannotTrainWithNoTrainers" );
        }

        private IEnumerator Test_CannotTrainWhenUnitAlreadyTrainedToMax() {
            SetTrainerCount( 1 );
            yield return mBackend.WaitUntilNotBusy();
            SetProgressData( 1, 1 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeAssignmentChange( 1 );

            FailTestIfClientInSync( "Test_CannotTrainWhenUnitAlreadyTrainedToMax" );
        }

        private IEnumerator Test_CannotUntrainWithZeroAssignedTrainers() {
            SetTrainerCount( 1 );
            yield return mBackend.WaitUntilNotBusy();
            SetProgressData( 1, 0 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeAssignmentChange( -1 );

            FailTestIfClientInSync( "Test_CannotUntrainWithZeroAssignedTrainers" );
        }

        private void FailTestIfAvailableTrainersDoesNotEqual( int i_count ) {
            mBackend.MakeCloudCall( "getAvailableTrainers", null, ( results ) => {
                if ( results.ContainsKey( "data" ) ) {
                    int count = int.Parse( results["data"] );
                    if ( count != i_count ) {
                        IntegrationTest.Fail( "Count did not match: " + i_count );
                    }
                }
                else {
                    IntegrationTest.Fail( "Results did not have data." );
                }
            } );
        }

        private void FailTestIfAssignedTrainersDoesNotEqual( int i_trainers ) {
            Dictionary<string, string> getParams = new Dictionary<string, string>();
            getParams.Add( "Class", "Units" );
            getParams.Add( "TargetID", UNIT_ID );

            mBackend.MakeCloudCall( "getProgressData", getParams, ( results ) => {
                if ( results.ContainsKey( "data" ) ) {
                    UnitProgress progress = JsonConvert.DeserializeObject<UnitProgress>( results["data"] );
                    if ( progress.Trainers != i_trainers ) {
                        IntegrationTest.Fail( "Trainers did not match: " + i_trainers );
                    }
                }
                else {
                    IntegrationTest.Fail( "Results did not have data." );
                }
            } );
        }

        private void SetTrainerCount( int i_trainers ) {
            SetPlayerData( TRAINER_DATA_KEY, DrsStringUtils.Replace( TRAINER_DATA, "NUM", i_trainers ) );
        }

        private void SetProgressData( int i_level, int i_trainers ) {
            string data = PROGRESS_DATA;
            data = DrsStringUtils.Replace( data, "LEVEL", i_level );
            data = DrsStringUtils.Replace( data, "TRAINERS", i_trainers );

            SetPlayerData( PROGRESS_KEY, data );
        }

        private IEnumerator MakeAssignmentChange( int i_change ) {
            mBackend.ChangeAssignedTrainers( UNIT_ID, i_change );
            yield return mBackend.WaitUntilNotBusy();
        }
    }
}