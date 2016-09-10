
namespace IdleFantasy.PlayFab.IntegrationTests {
    public class UnitShouldUnlockTest : UnitUnlockTestBase {
        protected override int GetExpectedLevel() {
            return 1;
        }

        protected override int GetTotalMissionsCompleted() {
            return 2;
        }
    }
}
