using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestCannotAddPointsAtMaxLevel : TestPointsUpgrades {
        private int mPointsToAdd = 100;
        private int mMaxLevel = 0;

        protected override IEnumerator RunTest() {
            yield return SetMaxLevel();
            yield return SetStartingSaveLevelAndData( mMaxLevel );

            yield return MakeAddPointsCall( mPointsToAdd );

            yield return FailTestIfPointsDoesNotEqual( 0 );
        }

        private IEnumerator SetMaxLevel() {
            yield return GetNumberFromCloudCall( CloudTestMethods.getMaxLevelForUpgrade.ToString(),
                new Dictionary<string, string>() { { BackendConstants.TARGET_ID, mCurrentTestData.TestID },
                    { BackendConstants.CLASS, mCurrentTestData.TestClass },
                    { BackendConstants.UPGRADE_ID, mCurrentTestData.TestUpgradeID } },
                ( result ) => {
                    mMaxLevel = (int) result;
                } );
        }
    }
}
