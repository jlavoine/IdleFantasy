using System.Diagnostics;

namespace MyLibrary {
    public class MyTimer : ITimer {
        private Stopwatch mStopwatch;

        public MyTimer() {
            mStopwatch = new Stopwatch();
        }

        public void Start() {
            mStopwatch.Start();
        }

        public void Stop() {
            mStopwatch.Stop();
        }

        public void Restart() {
            mStopwatch.Reset();
            mStopwatch.Start();
        }

        public long GetElapsedMilliseconds() {
            return mStopwatch.ElapsedMilliseconds;
        }
    }
}