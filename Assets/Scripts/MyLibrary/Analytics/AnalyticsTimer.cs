using System.Diagnostics;
using System.Collections.Generic;

namespace MyLibrary {
    public class AnalyticsTimer : IAnalyticsTimer {
        private string mAnalyticName;
        private Stopwatch mStopwatch;
        private long mTotalTime = 0;
        private Dictionary<string, object> mStepData =  new Dictionary<string, object>();

        public AnalyticsTimer( string i_analyticName ) {
            mAnalyticName = i_analyticName;
        }

        public void Start() {
            mStopwatch = new Stopwatch();
            mStopwatch.Start();
        }

        public void StepComplete( string i_stepName ) {
            AddStepToEventData( i_stepName );
            IncrementTotalTime();
            RestartTimer();            
        }

        private void AddStepToEventData( string i_stepName ) {
            mStepData.Add( i_stepName, mStopwatch.ElapsedMilliseconds );
        }

        private void IncrementTotalTime() {
            mTotalTime += mStopwatch.ElapsedMilliseconds;
        }

        private void RestartTimer() {
            mStopwatch.Reset();
            mStopwatch.Start();
        }

        public void Stop() {
            mStopwatch.Stop();
        }

        public void StopAndSendAnalytic() {
            IncrementTotalTime();
            mStopwatch.Stop();
            SendAnalytic();            
        }

        private void SendAnalytic() {
            mStepData.Add( LibraryAnalyticEvents.TOTAL_TIME, mTotalTime );

            MyMessenger.Send<string, IDictionary<string, object>>( LibraryAnalyticEvents.SEND_ANALYTIC_EVENT, mAnalyticName, mStepData );
        }
    }
}