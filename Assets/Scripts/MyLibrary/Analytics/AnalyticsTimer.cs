using System.Diagnostics;
using System.Collections.Generic;

namespace MyLibrary {
    public class AnalyticsTimer : IAnalyticsTimer {
        private string mAnalyticName;
        private long mTotalTime = 0;
        public long TotalTime { get { return mTotalTime; } }

        private ITimer mTimer;

        private Dictionary<string, object> mStepData =  new Dictionary<string, object>();
        public Dictionary<string, object> StepData { get { return mStepData; } }

        public AnalyticsTimer( string i_analyticName, ITimer i_timer ) {
            mTimer = i_timer;
            mAnalyticName = i_analyticName;
        }

        public void Start() {
            mTimer.Start();
        }

        public void StepComplete( string i_stepName ) {
            AddStepToEventData( i_stepName );
            IncrementTotalTime();
            RestartTimer();            
        }

        private void AddStepToEventData( string i_stepName ) {
            mStepData.Add( i_stepName, mTimer.GetElapsedMilliseconds() );
        }

        private void IncrementTotalTime() {
            mTotalTime += mTimer.GetElapsedMilliseconds();
        }

        private void RestartTimer() {
            mTimer.Restart();
        }

        public void StopTimer() {
            mTimer.Stop();
        }

        public void StopAndSendAnalytic() {
            IncrementTotalTime();
            StopTimer();
            SendAnalytic();            
        }

        private void SendAnalytic() {
            mStepData.Add( LibraryAnalyticEvents.TOTAL_TIME, mTotalTime );

            MyMessenger.Send<string, IDictionary<string, object>>( LibraryAnalyticEvents.SEND_ANALYTIC_EVENT, mAnalyticName, mStepData );
        }
    }
}