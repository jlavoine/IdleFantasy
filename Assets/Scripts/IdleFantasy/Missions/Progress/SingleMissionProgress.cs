
namespace IdleFantasy {
    public class SingleMissionProgress : ISingleMissionProgress {
        public bool Completed;       

        public bool IsComplete() { return Completed; }
    }
}
