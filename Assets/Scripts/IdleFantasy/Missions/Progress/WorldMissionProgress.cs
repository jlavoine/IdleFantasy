using System.Collections.Generic;

namespace IdleFantasy {
    public class WorldMissionProgress : IWorldMissionProgress {
        public string World;
        public List<SingleMissionProgress> Missions;

        public bool IsMissionWithIndexComplete( int i_index ) {
            if ( i_index < 0 || i_index >= Missions.Count ) {
                return false;
            } else {
                return Missions[i_index].Completed;
            }
        }

        public int GetCompletedMissionCount() {
            int count = 0;
            foreach ( SingleMissionProgress missionProgress in Missions ) {
                if ( missionProgress.Completed ) {
                    count++;
                }
            }

            return count;
        }

        public List<SingleMissionProgress> GetMissionProgress() {
            return Missions;
        }
    }
}
