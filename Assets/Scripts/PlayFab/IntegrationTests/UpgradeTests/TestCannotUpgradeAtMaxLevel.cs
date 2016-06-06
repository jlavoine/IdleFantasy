using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestCannotUpgradeAtMaxLevel : TestUpgrades {
        protected override IEnumerator RunTest() {
            IntegrationTestUtils.SetPlayerData( mCurrentTestData.SaveKey, DrsStringUtils.Replace( mCurrentTestData.SaveValue, "NUM", mCurrentTestData.MaxLevel ) );
            IntegrationTestUtils.SetPlayerCurrency( 100000 );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeUpgradeCall();

            FailTestIfClientInSync( "Test_CannotUpgradeAtMaxLevel" );
        }
    }
}


