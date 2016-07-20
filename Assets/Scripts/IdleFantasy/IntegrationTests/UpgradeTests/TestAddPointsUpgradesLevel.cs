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
            yield return GetNumberFromCloudCall( CloudTestMethods.getTotalPointsToUpgrade.ToString(),
                new Dictionary<string, string>() { { BackendConstants.TARGET_ID, mCurrentTestData.TestID },
                    { BackendConstants.CLASS, mCurrentTestData.TestClass },
                    { BackendConstants.UPGRADE_ID, mCurrentTestData.TestUpgradeID } },
                ( result ) => {
                    mPointsToAdd = (int) result;
                } );
        }
    }
}

