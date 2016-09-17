using System;
using System.Collections.Generic;
using MyLibrary;

namespace IdleFantasy {
    public class IdleFantasyOfflineBackend : OfflineBackend, IIdleFantasyBackend {
        public void ChangeAssignedTrainers( string i_unitID, int i_change ) {
            throw new NotImplementedException();
        }

        public void CompleteMission( string i_missionType, int i_missionIndex, Dictionary<int, MissionTaskProposal> i_taskProposals ) {
            throw new NotImplementedException();
        }

        public void GetMission( string i_missionType, Callback<MissionData> i_callback ) {
            throw new NotImplementedException();
        }

        public void MakeAddPointsToUpgradeCall( string i_className, string i_targetID, string i_upgradeID, int i_points ) {
            throw new NotImplementedException();
        }

        public void MakeAddProgressToUpgradeCall( string i_className, string i_targetID, string i_upgradeID, float i_progress ) {
            throw new NotImplementedException();
        }

        public void MakeTrainerPurchase() {
            throw new NotImplementedException();
        }

        public void MakeUpgradeCall( string i_className, string i_targetID, string i_upgradeID ) {
            throw new NotImplementedException();
        }

        public void SendTravelRequest( string i_world, int i_optionIndex ) {
            throw new NotImplementedException();
        }
    }
}