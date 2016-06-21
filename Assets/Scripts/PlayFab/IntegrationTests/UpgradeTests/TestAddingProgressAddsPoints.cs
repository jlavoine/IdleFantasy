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
            yield return GetNumberFromCloudCall( IdleFantasyBackend.TEST_GET_TOTAL_POINTS_UPGRADE,
                new Dictionary<string, string>() { { IntegrationTestUtils.TARGET_ID, mCurrentTestData.TestID },
                    { IntegrationTestUtils.CLASS, mCurrentTestData.TestClass },
                    { IntegrationTestUtils.UPGRADE_ID, mCurrentTestData.TestUpgradeID } },
                ( result ) => {
                    mTargetPoints = (int) ((int) result * mProgressToAdd);
                } );
        }
    }
}

