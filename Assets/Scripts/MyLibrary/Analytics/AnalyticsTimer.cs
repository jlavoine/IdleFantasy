using System.Diagnostics;

namespace MyLibrary {
    public class AnalyticsTimer {
        public const string TIMER_EVENT = "TimerEvent";

        private string mAnalyticName;
        private Stopwatch mStopwatch;
        private IMessageService mMessenger;

        public AnalyticsTimer( IMessageService i_messenger, string i_analyticName ) {
            mMessenger = i_messenger;
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

            mMessenger.Send<string, long>( TIMER_EVENT, mAnalyticName, mStopwatch.ElapsedMilliseconds );
        }
    }
}