using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestPointsBeyondMaxLevelAreZero : TestPointsUpgrades {
        private int mPointsToAdd = int.MaxValue;
        private int mLevelToSet = 0;

        protected override IEnumerator RunTest() {
            yield return SetLevelToSet();
            yield return SetStartingSaveLevelAndData( mLevelToSet );

            yield return MakeAddPointsCall( mPointsToAdd );

            yield return FailTestIfPointsDoesNotEqual( 0 );
        }

        private IEnumerator SetLevelToSet() {
            yield return GetNumberFromCloudCall( CloudTestMethods.getMaxLevelForUpgrade.ToString(),
                new Dictionary<string, string>() { { BackendConstants.TARGET_ID, mCurrentTestData.TestID },
                    { BackendConstants.CLASS, mCurrentTestData.TestClass },
                    { BackendConstants.UPGRADE_ID, mCurrentTestData.TestUpgradeID } },
                ( result ) => {
                    mLevelToSet = (int) result - 1;
                } );
        }

    }
}
