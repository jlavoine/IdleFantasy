using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestCanAffordNewTrainer : TestTrainerPurchases {
        protected override IEnumerator RunAllTests() {
            yield return CanAffordNewTrainer();
        }

        private IEnumerator CanAffordNewTrainer() {
            IntegrationTestUtils.SetReadOnlyData( SAVE_KEY, DrsStringUtils.Replace( SAVE_VALUE, "NUM", 1 ) );
            IntegrationTestUtils.SetPlayerCurrency( COST );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakePurchaseCall();

            FailTestIfCurrencyDoesNotEqual( 0 );
            FailTestIfReturnedCallDoesNotEqual( GET_TRAINER_COUNT_CLOUD_METHOD, 2 );

            yield return mBackend.WaitUntilNotBusy();
        }
    }
}
