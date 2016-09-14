
namespace MyLibrary {
    public interface IGameMetrics {
        int GetMetric( string i_metricName );
        void IncrementMetric( string i_metric );
    }
}
