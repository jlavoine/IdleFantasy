using System.Collections.Generic;

namespace IdleFantasy {
    public class TrainerSaveData {
        public Dictionary<string, int> TrainerCounts;

        public Dictionary<string, int> TrainerAssignments;

        public TrainerSaveData() {
            TrainerCounts = new Dictionary<string, int>();
            TrainerAssignments = new Dictionary<string, int>();
        }
    }
}
