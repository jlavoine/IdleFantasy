using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class TestTrainerPurchases : IntegrationTestBase {
        protected string SAVE_KEY = "TrainerSaveData";
        protected string SAVE_VALUE = "{\"TrainerCounts\":{\"Normal\":$NUM$}}";        

        protected int COST = 2000;

        protected IEnumerator MakePurchaseCall() {
            mBackend.MakeTrainerPurchase();
            yield return mBackend.WaitUntilNotBusy();
        }
    }
}