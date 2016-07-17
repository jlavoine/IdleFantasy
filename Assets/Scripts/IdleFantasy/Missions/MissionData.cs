using System.Collections.Generic;

namespace IdleFantasy {
    public class MissionData {
        public string MissionCategory;
        public int Index;
        public string DescriptionKey;
        public string RewardKey;

        public List<MissionTaskData> Tasks;
    }
}