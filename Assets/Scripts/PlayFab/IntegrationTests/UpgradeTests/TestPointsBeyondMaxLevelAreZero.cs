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
            yield return GetNumberFromCloudCall( IdleFantasyBackend.TEST_GET_UPGRADE_MAX_LEVEL,
                new Dictionary<string, string>() { { IntegrationTestUtils.TARGET_ID, mCurrentTestData.TestID },
                    { IntegrationTestUtils.CLASS, mCurrentTestData.TestClass },
                    { IntegrationTestUtils.UPGRADE_ID, mCurrentTestData.TestUpgradeID } },
                ( result ) => {
                    mLevelToSet = (int) result - 1;
                } );
        }

    }
}
