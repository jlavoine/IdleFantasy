using System.Collections.Generic;

namespace IdleFantasy {
    public class TrainerSaveData {
        public Dictionary<string, int> TrainerCounts;

        public TrainerSaveData() {
            TrainerCounts = new Dictionary<string, int>();
        }
    }
}
