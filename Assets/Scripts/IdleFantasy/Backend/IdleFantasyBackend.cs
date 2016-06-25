using MyLibrary;
using System.Collections.Generic;

namespace IdleFantasy {
    public class IdleFantasyBackend : PlayFabBackend, IBackend {
        public const string CHANGE = "Change";

        public const string INIT_UPGRADE = "initiateUpgrade";        
        public const string INIT_TRAINER_PURCHASE = "initiateTrainerPurchase";
        public const string INIT_TRAINING_CHANGE = "initiateChangeInTraining";
        public const string ADD_POINTS_TO_UPGRADE = "addPointsToUpgrade";
        public const string ADD_PROGRESS_TO_UPGRADE = "addProgressToUpgrade";

        public const string TEST_SET_DATA = "setSaveData";
        public const string TEST_SET_CURRENCY = "setPlayerCurrency";
        public const string TEST_UPDATE_UNIT_COUNT = "testUpdateUnitCount";
        public const string TEST_UPGRADE = "testUpgrade";
        public const string TEST_CHANGE_TRAINING = "testChangeTraining";
        public const string TEST_GET_UNIT_CAPACITY = "getCapacityForUnit";
        public const string TEST_GET_UNIT_TRAIN_TIME = "getTrainTimeForUnit";
        public const string TEST_GET_TOTAL_POINTS_UPGRADE = "getTotalPointsToUpgrade";
        public const string TEST_GET_UPGRADE_MAX_LEVEL = "getMaxLevelForUpgrade";

        public IdleFantasyBackend() : base() {
        }

        public void MakeUpgradeCall( string i_className, string i_targetID, string i_upgradeID ) {
            Dictionary<string, string> upgradeParams = new Dictionary<string, string>();
            upgradeParams.Add( "Class", i_className );
            upgradeParams.Add( "TargetID", i_targetID );
            upgradeParams.Add( "UpgradeID", i_upgradeID );

            MakeCloudCall( INIT_UPGRADE, upgradeParams, null );
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