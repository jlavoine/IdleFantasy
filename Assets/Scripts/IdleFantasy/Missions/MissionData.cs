using System.Collections.Generic;

namespace IdleFantasy {
    public class MissionData {
        public string MissionCategory;
        public int Index;
        public string DescriptionKey = "MISSING";
        public string RewardKey;
        public int GoldReward;

        public List<MissionTaskData> Tasks;
    }
}