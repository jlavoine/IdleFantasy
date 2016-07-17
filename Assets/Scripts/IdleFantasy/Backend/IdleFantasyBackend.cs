using MyLibrary;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy {
    public class IdleFantasyBackend : PlayFabBackend, IBackend {
        public const string CHANGE = "Change";

        public const string INIT_UPGRADE = "initiateUpgrade";        
        public const string INIT_TRAINER_PURCHASE = "initiateTrainerPurchase";
        public const string INIT_TRAINING_CHANGE = "initiateChangeInTraining";
        public const string ADD_POINTS_TO_UPGRADE = "addPointsToUpgrade";
        public const string ADD_PROGRESS_TO_UPGRADE = "addProgressToUpgrade";
        public const string GET_MISSIONS = "getTestMissions";
        public const string COMPLETE_MISSION = "initiateCompleteMission";

        public const string MISSION_TYPE_PARAM = "MissionCategory";
        public const string MISSION_INDEX_PARAM = "Index";
        public const string MISSOIN_PROPOSALS_PARAM = "TaskProposals";

        public IdleFantasyBackend() : base() {
        }

        public void MakeUpgradeCall( string i_className, string i_targetID, string i_upgradeID ) {
            Dictionary<string, string> upgradeParams = new Dictionary<string, string>();
            upgradeParams.Add( "Class", i_className );
            upgradeParams.Add( "TargetID", i_targetID );
            upgradeParams.Add( "UpgradeID", i_upgradeID );

            MakeCloudCall( INIT_UPGRADE, upgradeParams, null );
        }

        // right now this is just for testing
        public void GetMission(string i_missionType, Callback<MissionData> i_callback ) {
            Dictionary<string, string> cloudParams = new Dictionary<string, string>();
            cloudParams.Add( MISSION_TYPE_PARAM, i_missionType );

            MakeCloudCall( GET_MISSIONS, cloudParams, ( results ) => {
                if ( results.ContainsKey( "data" ) ) {
                    List<MissionData> listMissions = JsonConvert.DeserializeObject<List<MissionData>>( results["data"] );
                    i_callback( listMissions[0] );
                }
            } );
        }

        public void CompleteMission(string i_missionType, int i_missionIndex, Dictionary<int, MissionTaskProposal> i_taskProposals ) {
            Dictionary<string, string> cloudParams = new Dictionary<string, string>();
            cloudParams.Add( MISSION_TYPE_PARAM, i_missionType );
            cloudParams.Add( MISSION_INDEX_PARAM, i_missionIndex.ToString() );
            cloudParams.Add( MISSOIN_PROPOSALS_PARAM, JsonConvert.SerializeObject( i_taskProposals ) );

            MakeCloudCall( COMPLETE_MISSION, cloudParams, null );
        }

        public void MakeAddPointsToUpgradeCall( string i_className, string i_targetID, string i_upgradeID, int i_points ) {
            Dictionary<string, string> upgradeParams = new Dictionary<string, string>();
            upgradeParams.Add( "Class", i_className );
            upgradeParams.Add( "TargetID", i_targetID );
            upgradeParams.Add( "UpgradeID", i_upgradeID );
            upgradeParams.Add( "Points", i_points.ToString() );

            MakeCloudCall( ADD_POINTS_TO_UPGRADE, upgradeParams, null );
        }

        public void MakeAddProgressToUpgradeCall( string i_className, string i_targetID, string i_upgradeID, float i_progress ) {
            Dictionary<string, string> upgradeParams = new Dictionary<string, string>();
            upgradeParams.Add( "Class", i_className );
            upgradeParams.Add( "TargetID", i_targetID );
            upgradeParams.Add( "UpgradeID", i_upgradeID );
            upgradeParams.Add( "Progress", i_progress.ToString() );

            MakeCloudCall( ADD_PROGRESS_TO_UPGRADE, upgradeParams, null );
        }

        public void MakeTrainerPurchase() {
            MakeCloudCall( INIT_TRAINER_PURCHASE, null, null );
        }

        public void ChangeAssignedTrainers( string i_unitID, int i_change ) {
            Dictionary<string, string> assignParams = new Dictionary<string, string>();
            assignParams.Add( CHANGE, i_change.ToString() );
            assignParams.Add( "TargetID", i_unitID );

            MakeCloudCall( INIT_TRAINING_CHANGE, assignParams, null );
        }
    }
}