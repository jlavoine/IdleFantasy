using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestAddPointsUpgradesLevel : TestPointsUpgrades {
        private int mPointsToAdd = 0;
        private int mStartingLevel = 1;

        protected override IEnumerator RunTest() {
            yield return SetStartingSaveLevelAndData( mStartingLevel );

            yield return SetPointsToAdd();
            yield return MakeAddPointsCall( mPointsToAdd );

            yield return FailTestIfPointsDoesNotEqual( 0 );
            yield return FailTestIfNotProgressLevel<GuildProgress>( mCurrentTestData.TestClass, mCurrentTestData.TestID, mStartingLevel + 1 );
        }

        private IEnumerator SetPointsToAdd() {
            yield return GetNumberFromCloudCall( IdleFantasyBackend.TEST_GET_TOTAL_POINTS_UPGRADE,
                new Dictionary<string, string>() { { IntegrationTestUtils.TARGET_ID, mCurrentTestData.TestID },
                    { IntegrationTestUtils.CLASS, mCurrentTestData.TestClass },
                    { IntegrationTestUtils.UPGRADE_ID, mCurrentTestData.TestUpgradeID } },
                ( result ) => {
                    mPointsToAdd = (int) result;
                } );
        }
    }
}

