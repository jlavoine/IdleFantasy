using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestCannotAffordUpgrade : TestUpgrades {
        protected override IEnumerator RunTest() {
            SetPlayerData( mCurrentTestData.SaveKey, DrsStringUtils.Replace( mCurrentTestData.SaveValue, "NUM", 1 ) );
            SetPlayerCurrency( 0 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeUpgradeCall();

            FailTestIfClientInSync( "Test_CannotAffordUpgrade" );
        }
    }
}

