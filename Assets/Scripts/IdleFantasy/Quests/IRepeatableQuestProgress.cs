
namespace IdleFantasy {
    public interface IRepeatableQuestProgress {
        int GetCompletedCount();
        bool IsQuestAvailable();
        bool IsDone();
        MissionData GetMissionData();

        void SetMissionFinished();
    }
}
