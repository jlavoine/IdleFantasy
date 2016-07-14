
namespace IdleFantasy {
    public class MissionTaskData : IMissionTaskData {
        public string DescriptionKey;
        public string DescriptionKey_ { get { return DescriptionKey; } }

        public string StatRequirement;
        public string StatRequirement_ { get { return StatRequirement; } }

        public int PowerRequirement;        
        public int PowerRequirement_ { get { return PowerRequirement; } }
    }
}
