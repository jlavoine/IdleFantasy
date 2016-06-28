using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestAddPointsToUpgrade : TestPointsUpgrades {
        private int mPointsToAdd = 0;

        protected override IEnumerator RunTest() {
            yield return SetStartingSaveLevelAndData( 1 );
            
            yield return SetPointsToAdd();
            yield return MakeAddPointsCall( mPointsToAdd );

            yield return  FailTestIfPointsDoesNotEqual( mPointsToAdd );
        }

        private IEnumerator SetPointsToAdd() {
            yield return GetNumberFromCloudCall( CloudTestMethods.getTotalPointsToUpgrade.ToString(),
                new Dictionary<string, string>() { { IntegrationTestUtils.TARGET_ID, mCurrentTestData.TestID },
                    { IntegrationTestUtils.CLASS, mCurrentTestData.TestClass },
                    { IntegrationTestUtils.UPGRADE_ID, mCurrentTestData.TestUpgradeID } },
                ( result ) => {                    
                    mPointsToAdd = (int)result / 2; // don't want all the points so the upgrade doesn't level up!
                } );
        }
    }
}
