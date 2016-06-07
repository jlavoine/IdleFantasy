using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestCannotAffordUpgrade : TestUpgrades {
        protected override IEnumerator RunTest() {
            IntegrationTestUtils.SetReadOnlyData( mCurrentTestData.SaveKey, DrsStringUtils.Replace( mCurrentTestData.SaveValue, "NUM", 1 ) );
            IntegrationTestUtils.SetPlayerCurrency( 0 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeUpgradeCall();

            FailTestIfClientInSync( "Test_CannotAffordUpgrade" );
        }
    }
}

