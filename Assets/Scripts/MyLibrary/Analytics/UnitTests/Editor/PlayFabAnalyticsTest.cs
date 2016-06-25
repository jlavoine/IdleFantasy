using NUnit.Framework;
using NSubstitute;

#pragma warning disable 0414

namespace MyLibrary.UnitTests {
    [TestFixture]
    public class PlayFabAnalyticsTest  {

        [Test]
        public void TimerAnalyticIsCalled_OnMessage() {
            PlayFabAnalytics analytics = new PlayFabAnalytics();
            string testName = "Test";
            long testTime = 0;

            MyMessenger.Send<string, long>( AnalyticsTimer.TIMER_EVENT, testName, testTime );

            Received.InOrder( () => {
                analytics.OnTimerAnalytic( testName, testTime );
            } );
        }
    }
}
