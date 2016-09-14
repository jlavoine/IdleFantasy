using System.Collections.Generic;

namespace MyLibrary {
    public class GameMetrics : IGameMetrics {
        public Dictionary<string, int> Metrics;

        public int GetMetric( string i_metric ) {
            AddMetricIfMissing( i_metric );            
            return Metrics[i_metric];
        }

        public void IncrementMetric( string i_metric ) {
            AddMetricIfMissing( i_metric );
            Metrics[i_metric]++;

            MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Incremented " + i_metric + " to " + Metrics[i_metric], "GameMetrics" );
        }

        private void AddMetricIfMissing( string i_metric ) {
            if ( !Metrics.ContainsKey( i_metric ) ) {
                Metrics[i_metric] = 0;
            }
        }
    }
}