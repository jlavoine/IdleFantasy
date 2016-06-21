using UnityEngine;
using System.Collections;
namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestAddingProgressUpgradesLevel : TestPointsUpgrades {
        private float mProgressToAdd = 2f;
        private int mExpectedLevel = 3;

        protected override IEnumerator RunTest() {
            yield return SetStartingSaveLevelAndData( 1 );

            yield return MakeAddProgressCall( mProgressToAdd );

            yield return FailTestIfPointsDoesNotEqual( 0 );
            yield return FailTestIfNotProgressLevel<GuildProgress>( mCurrentTestData.TestClass, mCurrentTestData.TestID, mExpectedLevel );
        }
    }
}

