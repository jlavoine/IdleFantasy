
namespace IdleFantasy.PlayFab.IntegrationTests {
    public class UnitShouldNotUnlockTest : UnitUnlockTestBase {
        protected override int GetExpectedLevel() {
            return 0;
        }

        protected override int GetTotalMissionsCompleted() {
            return 0;
        }
    }
}