using UnityEngine;
using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestCanAffordUpgrade : TestStandardUpgrades {
        protected override IEnumerator RunTest() {
            IntegrationTestUtils.SetReadOnlyData( mCurrentTestData.SaveKey, DrsStringUtils.Replace( mCurrentTestData.SaveValue, "NUM", 1 ) );
            IntegrationTestUtils.SetPlayerCurrency( mCurrentTestData.Cost );

            yield return mBackend.WaitUntilNotBusy();

            yield return MakeUpgradeCall();

            FailTestIfCurrencyDoesNotEqual( 0 );
            FailTestIfNotProgressLevel<ProgressBase>( mCurrentTestData.TestClass, mCurrentTestData.TestID, 2 );            
        }
    }
}
