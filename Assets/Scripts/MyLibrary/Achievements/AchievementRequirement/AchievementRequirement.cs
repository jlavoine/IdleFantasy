
namespace MyLibrary {
    public class AchievementRequirement : IAchievementRequirement {
        public string MetricName;
        public int RequiredCount;

        private IGameMetrics mMetrics;

        public AchievementRequirement( string i_metricName, int i_requiredCount, IGameMetrics i_metrics ) {
            MetricName = i_metricName;
            RequiredCount = i_requiredCount;
            mMetrics = i_metrics;
        }

        public bool DoesPass() {
            return mMetrics.GetMetric( MetricName ) >= RequiredCount;
        }
    }
}