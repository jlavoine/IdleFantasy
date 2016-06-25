using System.Diagnostics;

namespace MyLibrary {
    public class AnalyticsTimer {
        public const string TIMER_EVENT = "TimerEvent";

        private string mAnalyticName;
        private Stopwatch mStopwatch;

        public AnalyticsTimer( string i_analyticName ) {
            mAnalyticName = i_analyticName;
        }

        public void Start() {
            mStopwatch = new Stopwatch();
            mStopwatch.Start();
        }

        public void Stop() {
            mStopwatch.Stop();
        }

        public void StopAndSend() {
            mStopwatch.Stop();

            MyMessenger.Send<string, long>( TIMER_EVENT, mAnalyticName, mStopwatch.ElapsedMilliseconds );
        }
    }
}