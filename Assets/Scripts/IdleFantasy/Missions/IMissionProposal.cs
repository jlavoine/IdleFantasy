using System.Collections.Generic;

namespace IdleFantasy {
    public interface IMissionProposal {
        Dictionary<string, int> PromisedUnits { get; }
        Dictionary<int, MissionTaskProposal> TaskProposals { get; }

        void AddProposal( int i_taskIndex, MissionTaskProposal i_taskProposal );
        void RemoveProposal( int i_taskIndex, MissionTaskProposal i_taskProposal );
    }
}
