
namespace IdleFantasy {
    public class MissionTaskProposal {
        public int TaskIndex;
        public string UnitID;
        public int UnitCount;

        public MissionTaskProposal( int i_index, string i_id, int i_count ) {
            TaskIndex = i_index;
            UnitID = i_id;
            UnitCount = i_count;
        }
    }
}
