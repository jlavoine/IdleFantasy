using System.Collections.Generic;

namespace IdleFantasy {
    public interface IIdleFantasyBackend : IBackend {
        void GetMission( string i_missionType, Callback<MissionData> i_callback );
        void CompleteMission( string i_missionType, int i_missionIndex, Dictionary<int, MissionTaskProposal> i_taskProposals );
        void SendTravelRequest( string i_world, int i_optionIndex );
        void MakeAddPointsToUpgradeCall( string i_className, string i_targetID, string i_upgradeID, int i_points );
        void MakeAddProgressToUpgradeCall( string i_className, string i_targetID, string i_upgradeID, float i_progress );
        void MakeTrainerPurchase();
        void ChangeAssignedTrainers( string i_unitID, int i_change );
    }
}
