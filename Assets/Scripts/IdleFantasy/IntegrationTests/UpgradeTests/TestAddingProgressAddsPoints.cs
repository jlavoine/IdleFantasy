using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestAddingProgressAddsPoints : TestPointsUpgrades {
        private float mProgressToAdd = .5f;
        private int mTargetPoints;

        protected override IEnumerator RunTest() {
            yield return SetStartingSaveLevelAndData( 1 );

            yield return SetTargetPoints();
            yield return MakeAddProgressCall( mProgressToAdd );

            yield return FailTestIfPointsDoesNotEqual( mTargetPoints );
        }

        private IEnumerator SetTargetPoints() {
            yield return GetNumberFromCloudCall( CloudTestMethods.getTotalPointsToUpgrade.ToString(),
                new Dictionary<string, string>() { { BackendConstants.TARGET_ID, mCurrentTestData.TestID },
                    { BackendConstants.CLASS, mCurrentTestData.TestClass },
                    { BackendConstants.UPGRADE_ID, mCurrentTestData.TestUpgradeID } },
                ( result ) => {
                    mTargetPoints = (int) ((int) result * mProgressToAdd);
                } );
        }
    }
}

