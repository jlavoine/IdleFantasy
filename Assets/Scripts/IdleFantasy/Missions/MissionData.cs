using System.Collections.Generic;

namespace IdleFantasy {
    public class MissionData {
        public string ID;
        public string DescriptionKey;
        public string RewardKey;

        public List<MissionTaskData> Tasks;
    }
}