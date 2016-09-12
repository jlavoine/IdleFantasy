using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;
using UnityEngine.Analytics;

#pragma warning disable 0414

namespace MyLibrary.UnitTests {
    [TestFixture]
    public class AnalyticsManagerTests {
        private const string ANALYTIC_NAME = "Test";
        private Dictionary<string, object> ANALYTIC_DATA = new Dictionary<string, object>();

        private UnityAnalyticsManager mManagerUnderTest;
        private ILogService mLogger;

        [SetUp]
        public void BeforeTests() {
            mLogger = LibraryTestUtils.ReplaceLogger();
        }

        [TearDown]
        public void AfterTests() {
            LibraryTestUtils.ResetLogger();
            mManagerUnderTest.Dispose();
        }

        [Test]
        public void OnSendMessage_CustomAnalyticIsSent() {
            IUnityAnalytics okAnalytics = GetAnalyticsForResult( AnalyticsResult.Ok );
            mManagerUnderTest = new UnityAnalyticsManager( okAnalytics );
            MyMessenger.Send<string, IDictionary<string, object>>( LibraryAnalyticEvents.SEND_ANALYTIC_EVENT, ANALYTIC_NAME, ANALYTIC_DATA );

            okAnalytics.Received().SendCustomEvent( ANALYTIC_NAME, ANALYTIC_DATA );
        }

        [Test]
        public void OnSendMessage_InfoLogIsSent() {
            IUnityAnalytics okAnalytics = GetAnalyticsForResult( AnalyticsResult.Ok );
            mManagerUnderTest = new UnityAnalyticsManager( okAnalytics );
            MyMessenger.Send<string, IDictionary<string, object>>( LibraryAnalyticEvents.SEND_ANALYTIC_EVENT, ANALYTIC_NAME, ANALYTIC_DATA );

            EasyLogger.Instance.Received().Log( LogTypes.Info, Arg.Any<string>(), Arg.Any<string>() );
        }

        [Test]
        public void OnSentMessage_OkAnalytic_NoWarningLog() {
            IUnityAnalytics okAnalytics = GetAnalyticsForResult( AnalyticsResult.Ok );
            mManagerUnderTest = new UnityAnalyticsManager( okAnalytics );
            MyMessenger.Send<string, IDictionary<string, object>>( LibraryAnalyticEvents.SEND_ANALYTIC_EVENT, ANALYTIC_NAME, ANALYTIC_DATA );

            EasyLogger.Instance.DidNotReceive().Log( LogTypes.Warn, Arg.Any<string>(), Arg.Any<string>() );
        }

        [Test]
        public void OnSentMessage_FailedAnalytic_WarningLogSent() {
            IUnityAnalytics badAnalytics = GetAnalyticsForResult( AnalyticsResult.InvalidData );
            mManagerUnderTest = new UnityAnalyticsManager( badAnalytics );
            MyMessenger.Send<string, IDictionary<string, object>>( LibraryAnalyticEvents.SEND_ANALYTIC_EVENT, ANALYTIC_NAME, ANALYTIC_DATA );

            EasyLogger.Instance.Received().Log( LogTypes.Warn, Arg.Any<string>(), Arg.Any<string>() );
        }

        private IUnityAnalytics GetAnalyticsForResult( AnalyticsResult i_result ) {
            IUnityAnalytics analytics = Substitute.For<IUnityAnalytics>();
            analytics.SendCustomEvent( Arg.Any<string>(), Arg.Any<IDictionary<string, object>>() ).Returns( i_result );
            return analytics;
        }
    }
}
