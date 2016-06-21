using System.Collections.Generic;
using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public abstract class TestPointsUpgrades : TestUpgrades {

        protected override void SetTestData() {
            mUpgradeTests = new List<UpgradeTestData>();

            SetGuildTestData();
        }

        private void SetGuildTestData() {
            UpgradeTestData testData = new UpgradeTestData();
            testData.SaveKey = "GuildsProgress";
            testData.SaveValue = "{\"GUILD_1\":{\"Level\":$LEVEL$,\"Points\":0}}";
            testData.TestID = "GUILD_1";
            testData.TestClass = "Guilds";
            testData.TestUpgradeID = "GuildLevel";
            testData.MaxLevel = 50;
            testData.Points = 0;
            mUpgradeTests.Add( testData );
        }

        protected IEnumerator MakeAddPointsCall( int i_points ) {
            mBackend.MakeAddPointsToUpgradeCall( mCurrentTestData.TestClass, mCurrentTestData.TestID, mCurrentTestData.TestUpgradeID, i_points );
            yield return mBackend.WaitUntilNotBusy();
        }

        protected IEnumerator MakeAddProgressCall( float i_progress ) {
            mBackend.MakeAddProgressToUpgradeCall( mCurrentTestData.TestClass, mCurrentTestData.TestID, mCurrentTestData.TestUpgradeID, i_progress );
            yield return mBackend.WaitUntilNotBusy();
        }

        protected IEnumerator SetStartingSaveLevelAndData( int i_level ) {
            mCurrentTestData.SaveValue = DrsStringUtils.Replace( mCurrentTestData.SaveValue, "LEVEL", i_level );

            IntegrationTestUtils.SetReadOnlyData( mCurrentTestData.SaveKey, mCurrentTestData.SaveValue );

            yield return mBackend.WaitUntilNotBusy();
        }

        protected IEnumerator FailTestIfPointsDoesNotEqual( int i_points ) {
            GetProgressData<GuildProgress>( GenericDataLoader.GUILDS, mCurrentTestData.TestID, ( result ) => {
                if ( result.Points != i_points ) {
                    IntegrationTest.Fail( "Expecting points to be " + i_points + " but was " + result.Points );
                }
            } );

            yield return mBackend.WaitUntilNotBusy();
        }
    }
}
