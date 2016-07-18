using System.Collections.Generic;

namespace IdleFantasy {
    public class MissionProposal {
        private Dictionary<string, int> mPromisedUnits = new Dictionary<string, int>();
        public Dictionary<string, int> PromisedUnits { get { return mPromisedUnits; } }

        private Dictionary<int, MissionTaskProposal> mTaskProposals = new Dictionary<int, MissionTaskProposal>();
        public Dictionary<int, MissionTaskProposal> TaskProposals { get { return mTaskProposals; } }

        public MissionProposal() { }
        public MissionProposal( Dictionary<string, int> i_promisedUnits, Dictionary<int, MissionTaskProposal> i_proposals ) {
            mPromisedUnits = i_promisedUnits;
            mTaskProposals = i_proposals;
        }

        public void AddProposal( int i_taskIndex, MissionTaskProposal i_taskProposal ) {
            SetTaskProposal( i_taskIndex, i_taskProposal );            
            ChangePromisedUnits( i_taskProposal.UnitID, i_taskProposal.UnitCount );
        }

        public void RemoveProposal( int i_taskIndex, MissionTaskProposal i_taskProposal ) {
            SetTaskProposal( i_taskIndex, null );
            ChangePromisedUnits( i_taskProposal.UnitID, -i_taskProposal.UnitCount );
        }

        private void SetTaskProposal( int i_taskIndex, MissionTaskProposal i_taskProposal ) {
            mTaskProposals[i_taskIndex] = i_taskProposal;
        }

        private void ChangePromisedUnits( string i_unitID, int amount ) {
            int promisedUnits = 0;
            mPromisedUnits.TryGetValue( i_unitID, out promisedUnits );
            promisedUnits += amount;
            mPromisedUnits[i_unitID] = promisedUnits;
        }
    }
}
