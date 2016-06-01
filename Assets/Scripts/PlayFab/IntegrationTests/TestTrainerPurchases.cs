using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestTrainerPurchases : IntegrationTestBase {
        private string SAVE_KEY = "TrainerSaveData";
        private string SAVE_VALUE = "{\"TrainerCounts\":{\"Normal\":$NUM$}}";

        private const string GET_TRAINER_COUNT_CLOUD_METHOD = "getTrainerCount";

        private int COST = 2000;

        protected override IEnumerator RunAllTests() {
            yield return mBackend.WaitUntilNotBusy();

            yield return Test_CanAffordNewTrainer();
            yield return Test_CannotAffordNewTrainer();

            DoneWithTests();
        }

        private IEnumerator Test_CanAffordNewTrainer() {
            SetPlayerData( SAVE_KEY, DrsStringUtils.Replace( SAVE_VALUE, "NUM", 1 ) );
            SetPlayerCurrency( COST );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakePurchaseCall();

            FailTestIfCurrencyDoesNotEqual( 0 );
            FailTestIfReturnedCallDoesNotEqual( GET_TRAINER_COUNT_CLOUD_METHOD, 2 );

            yield return mBackend.WaitUntilNotBusy();
        }

        private IEnumerator Test_CannotAffordNewTrainer() {
            SetPlayerData( SAVE_KEY, DrsStringUtils.Replace( SAVE_VALUE, "NUM", 1 ) );
            SetPlayerCurrency( 0 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakePurchaseCall();

            FailTestIfClientInSync( "Test_CannotNewTrainer" );
        }

        private IEnumerator MakePurchaseCall() {
            mBackend.MakeTrainerPurchase();
            yield return mBackend.WaitUntilNotBusy();
        }
    }
}