// cloud call limit is 10 calls; adding twenty levels is well beyond that

using System.Collections;
namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestAddingTwentyLevelsPassesCloudLimit : TestPointsUpgrades {
        private float mProgressToAdd = 20f;
        private int mExpectedLevel = 21;

        protected override IEnumerator RunTest() {
            yield return SetStartingSaveLevelAndData( 1 );

            yield return MakeAddProgressCall( mProgressToAdd );

            yield return FailTestIfPointsDoesNotEqual( 0 );
            yield return FailTestIfNotProgressLevel<GuildProgress>( mCurrentTestData.TestClass, mCurrentTestData.TestID, mExpectedLevel );
        }
    }
}


