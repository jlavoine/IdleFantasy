using System.Collections.Generic;

namespace IdleFantasy {
    public class WorldMissionProgress : IWorldMissionProgress {
        public string World;
        public List<SingleMissionProgress> Missions;

        public int GetCompletedMissionCount() {
            int count = 0;
            foreach ( SingleMissionProgress missionProgress in Missions ) {
                if ( missionProgress.Completed ) {
                    count++;
                }
            }

            return count;
        }
    }
}
