using System.Collections;
using System;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestCloudscriptTestMethodsOnlyForTesting : IntegrationTestBase {
        protected override IEnumerator RunAllTests() {
            foreach ( CloudTestMethods testMethod in Enum.GetValues( typeof( CloudTestMethods ) ) ) {
                mBackend.MakeCloudCall( testMethod.ToString(), null, null );

                yield return mBackend.WaitUntilNotBusy();

                // this exact thing will probably never happen.
                // what's more likely to happen is that the test will just fail if the method isn't protected by isTesting()
                // because the method will try to get arguments that were not passed in.
                FailTestIfClientInSync( "Test method not protected: " + testMethod.ToString() );
            }
        }
    }
}
