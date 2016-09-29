using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestDataWipedOnIncompleteIntroTutorial : TestDataWipedBase {
        protected override IEnumerator RunAllTests() {
            yield return DeleteAllPlayerSaveData();
            yield return VerifyPlayerSaveDataIsEmpty();
            yield return SetSomePlayerSaveData();
            yield return AddMissingSaveData();
            yield return VerifySaveDataIsDefault();
        }

        private IEnumerator SetSomePlayerSaveData() {
            yield return IntegrationTestUtils.SetPlayerCurrencyAndWait( 1000 );

            IntegrationTestUtils.SetReadOnlyData( IntegrationTestUtils.SAVE_KEY_GUILDS, "{\"GUILD_1\":{\"Level\":1,\"Points\":0}}" );

            yield return mBackend.WaitUntilNotBusy();
        }
    }
}