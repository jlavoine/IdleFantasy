using System.Collections;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class WorldResetFailsWhenTooLowMapLevel : WorldResetTestsBase {
        protected override int GetMapLevel() {
            return 1;
        }

        protected override bool IsTestExpectedToFail() {
            return true;
        }

        protected override IEnumerator RunOtherFailureChecks() {
            yield return null;
        }
    }
}
