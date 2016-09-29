using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestAddMissingData : TestDataWipedBase {
        protected override IEnumerator RunAllTests() {
            yield return DeleteAllPlayerSaveData();
            yield return VerifyPlayerSaveDataIsEmpty();
            yield return AddMissingSaveData();
            yield return VerifySaveDataIsDefault();
        }
    }
}
