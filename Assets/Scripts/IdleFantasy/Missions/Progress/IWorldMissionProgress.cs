using System.Collections.Generic;

namespace IdleFantasy {
    public interface IWorldMissionProgress {
        int GetCompletedMissionCount();
        List<SingleMissionProgress> GetMissionProgress();
        bool IsMissionWithIndexComplete( int i_index );
    }
}