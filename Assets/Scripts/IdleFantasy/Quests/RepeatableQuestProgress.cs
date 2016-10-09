
namespace IdleFantasy {
    public class RepeatableQuestProgress : IRepeatableQuestProgress {
        public string World;        
        public int CompletedCount;

        public bool CurrentlyAvailable;
        public bool DidMission = false;

        public MissionData Mission;

        public int GetCompletedCount() {
            return CompletedCount;
        }

        public bool IsQuestAvailable() {
            return CurrentlyAvailable;
        }

        public bool IsDone() {
            return DidMission;
        }

        public MissionData GetMissionData() {
            return Mission;
        }

        public void SetMissionFinished() {
            DidMission = true;
        }
    }
}