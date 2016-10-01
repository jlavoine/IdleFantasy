
using MyLibrary;

namespace IdleFantasy {
    public class IdleFantasyTutorial : Tutorial {
        protected override bool ShouldStartTutorial() {
            IGameMetrics metrics = PlayerManager.Data.GameMetrics;
            return metrics.GetMetric( TutorialName ) == 0;
        }
    }
}
