using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestCannotAffordNewTrainer : TestTrainerPurchases {
        protected override IEnumerator RunAllTests() {
            yield return CannotAffordNewTrainer();
        }

        private IEnumerator CannotAffordNewTrainer() {
            SetPlayerData( SAVE_KEY, DrsStringUtils.Replace( SAVE_VALUE, "NUM", 1 ) );
            SetPlayerCurrency( 0 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakePurchaseCall();

            FailTestIfClientInSync( "CannotAffordNewTrainer" );
        }
    }
}

