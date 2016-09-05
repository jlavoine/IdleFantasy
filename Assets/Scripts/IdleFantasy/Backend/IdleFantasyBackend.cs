using MyLibrary;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace IdleFantasy {
    public class IdleFantasyBackend : PlayFabBackend, IIdleFantasyBackend {
        public IdleFantasyBackend() : base() {}

        public void MakeUpgradeCall( string i_className, string i_targetID, string i_upgradeID ) {
            Dictionary<string, string> upgradeParams = new Dictionary<string, string>();
            upgradeParams.Add( BackendConstants.CLASS, i_className );
            upgradeParams.Add( BackendConstants.TARGET_ID, i_targetID );
            upgradeParams.Add( BackendConstants.UPGRADE_ID, i_upgradeID );

            MakeCloudCall( BackendConstants.INIT_UPGRADE, upgradeParams, null );
        }

        // right now this is just for testing
        public void GetMission(string i_missionType, Callback<MissionData> i_callback ) {
            Dictionary<string, string> cloudParams = new Dictionary<string, string>();
            cloudParams.Add( BackendConstants.MISSION_TYPE, i_missionType );

            MakeCloudCall( BackendConstants.GET_MISSIONS, cloudParams, ( results ) => {
                if ( results.ContainsKey( BackendConstants.DATA ) ) {
                    List<MissionData> listMissions = JsonConvert.DeserializeObject<List<MissionData>>( results[BackendConstants.DATA] );
                    i_callback( listMissions[0] );
                }
            } );
        }

        public void CompleteMission(string i_missionType, int i_missionIndex, Dictionary<int, MissionTaskProposal> i_taskProposals ) {            
            Dictionary<string, string> cloudParams = new Dictionary<string, string>();
            cloudParams.Add( BackendConstants.MISSION_TYPE, i_missionType );
            cloudParams.Add( BackendConstants.MISSION_INDEX, i_missionIndex.ToString() );
            cloudParams.Add( BackendConstants.MISSION_PROPOSALS, JsonConvert.SerializeObject( i_taskProposals ) );            

            MakeCloudCall( BackendConstants.COMPLETE_MISSION, cloudParams, null );
        }  

        public void MakeAddPointsToUpgradeCall( string i_className, string i_targetID, string i_upgradeID, int i_points ) {
            Dictionary<string, string> upgradeParams = new Dictionary<string, string>();
            upgradeParams.Add( BackendConstants.CLASS, i_className );
            upgradeParams.Add( BackendConstants.TARGET_ID, i_targetID );
            upgradeParams.Add( BackendConstants.UPGRADE_ID, i_upgradeID );
            upgradeParams.Add( BackendConstants.POINTS, i_points.ToString() );

            MakeCloudCall( BackendConstants.ADD_POINTS_TO_UPGRADE, upgradeParams, null );
        }

        public void MakeAddProgressToUpgradeCall( string i_className, string i_targetID, string i_upgradeID, float i_progress ) {
            Dictionary<string, string> upgradeParams = new Dictionary<string, string>();
            upgradeParams.Add( BackendConstants.CLASS, i_className );
            upgradeParams.Add( BackendConstants.TARGET_ID, i_targetID );
            upgradeParams.Add( BackendConstants.UPGRADE_ID, i_upgradeID );
            upgradeParams.Add( BackendConstants.PROGRESS, i_progress.ToString() );

            MakeCloudCall( BackendConstants.ADD_PROGRESS_TO_UPGRADE, upgradeParams, null );
        }

        public void MakeTrainerPurchase() {
            MakeCloudCall( BackendConstants.INIT_TRAINER_PURCHASE, null, null );
        }

        public void ChangeAssignedTrainers( string i_unitID, int i_change ) {
            Dictionary<string, string> assignParams = new Dictionary<string, string>();
            assignParams.Add( BackendConstants.CHANGE, i_change.ToString() );
            assignParams.Add( BackendConstants.TARGET_ID, i_unitID );

            MakeCloudCall( BackendConstants.INIT_TRAINING_CHANGE, assignParams, null );
        }
    }
}