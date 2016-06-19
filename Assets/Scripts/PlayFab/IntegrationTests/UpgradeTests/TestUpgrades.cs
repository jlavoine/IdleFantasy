using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class TestUpgrades : IntegrationTestBase {
        protected List<UpgradeTestData> mUpgradeTests;
        protected UpgradeTestData mCurrentTestData;

        protected abstract void SetTestData();
        protected abstract IEnumerator RunTest();

        protected override IEnumerator RunAllTests() {
            SetTestData();

            foreach ( UpgradeTestData testData in mUpgradeTests ) {
                mCurrentTestData = testData;

                yield return RunTest();

                yield return mBackend.WaitUntilNotBusy();
            }
        }        
    }
}