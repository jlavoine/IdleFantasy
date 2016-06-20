using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestCannotAddNegativePoints : TestPointsUpgrades {
        private int mPointsToAdd = -1;

        protected override IEnumerator RunTest() {
            yield return SetStartingSaveLevelAndData( 1 );

            yield return MakeAddPointsCall( mPointsToAdd );

            yield return FailTestIfPointsDoesNotEqual( 0 );
        }
    }
}
