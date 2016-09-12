using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;

#pragma warning disable 0414

namespace MyLibrary.UnitTests {
    [TestFixture]
    public class AnalyticsTimerTests {
        private const string ANALYTIC_NAME = "Test";
        private const string STEP_1 = "Step1";
        private const string STEP_2 = "Step2";
        private const long TIME_PER_STEP = 1000;

        private AnalyticsTimer mAnalyticsTimerUnderTest;
        private ITimer mTimer;
        private IUnityAnalyticsManager mAnalyticsManager;
        private IUnityAnalytics mAnalytics;

        [SetUp]
        public void BeforeTest() {
            mTimer = Substitute.For<ITimer>();
            mAnalyticsTimerUnderTest = new AnalyticsTimer( ANALYTIC_NAME, mTimer );

            mAnalytics = Substitute.For<IUnityAnalytics>();
            mAnalyticsManager = new UnityAnalyticsManager( mAnalytics );
        }

        [TearDown]
        public void AfterTest() {
            mAnalyticsManager.Dispose();
        }
        
        [Test]
        public void StartTimer_BeginsTiming() {
            mAnalyticsTimerUnderTest.Start();

            mTimer.Received().Start();
        }

        [Test]
        public void CompletingStep_AddsStepToEventData() {
            mAnalyticsTimerUnderTest.StepComplete( STEP_1 );

            Assert.Contains( STEP_1, mAnalyticsTimerUnderTest.StepData.Keys );
        }

        [Test]
        public void CompletingStep_RestartsTimer() {
            mAnalyticsTimerUnderTest.StepComplete( STEP_1 );

            mTimer.Received().Restart();
        }

        [Test]
        public void StopAndSend_StopsTimer() {
            mAnalyticsTimerUnderTest.StopAndSendAnalytic();

            mTimer.Received().Stop();
        }

        [Test]
        public void StopAndSend_SendsAnalytic() {
            mAnalyticsTimerUnderTest.StopAndSendAnalytic();

            mAnalytics.Received().SendCustomEvent( ANALYTIC_NAME, Arg.Any<IDictionary<string, object>>() );
        }

        [Test]
        public void TimerTracksTimeAccordingToTimer() {
            mTimer.GetElapsedMilliseconds().Returns( TIME_PER_STEP );
            mAnalyticsTimerUnderTest.Start();
            mAnalyticsTimerUnderTest.StepComplete( STEP_1 );
            mAnalyticsTimerUnderTest.StepComplete( STEP_2 );
            mAnalyticsTimerUnderTest.StopAndSendAnalytic();

            Assert.AreEqual( TIME_PER_STEP, mAnalyticsTimerUnderTest.StepData[STEP_1] );
            Assert.AreEqual( TIME_PER_STEP, mAnalyticsTimerUnderTest.StepData[STEP_2] );
            Assert.AreEqual( TIME_PER_STEP * 3, mAnalyticsTimerUnderTest.TotalTime );
        }
    }
}
