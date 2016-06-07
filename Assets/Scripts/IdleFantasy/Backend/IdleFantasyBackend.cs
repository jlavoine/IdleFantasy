using MyLibrary;
using System.Collections.Generic;

namespace IdleFantasy {
    public class IdleFantasyBackend : PlayFabBackend, IBackend {
        public const string CHANGE = "Change";

        public const string INIT_UPGRADE = "initiateUpgrade";
        public const string INIT_TRAINER_PURCHASE = "initiateTrainerPurchase";
        public const string INIT_TRAINING_CHANGE = "initiateChangeInTraining";

        public const string TEST_SET_DATA = "setSaveData";
        public const string TEST_SET_CURRENCY = "setPlayerCurrency";
        public const string TEST_UPDATE_UNIT_COUNT = "testUpdateUnitCount";
        public const string TEST_UPGRADE = "testUpgrade";
        public const string TEST_CHANGE_TRAINING = "testChangeTraining";

        public IdleFantasyBackend( IMessageService i_messenger ) : base(i_messenger) {
            mMessenger = i_messenger;
        }

        public void MakeUpgradeCall( string i_className, string i_targetID, string i_upgradeID ) {
            Dictionary<string, string> upgradeParams = new Dictionary<string, string>();
            upgradeParams.Add( "Class", i_className );
            upgradeParams.Add( "TargetID", i_targetID );
            upgradeParams.Add( "UpgradeID", i_upgradeID );

            MakeCloudCall( INIT_UPGRADE, upgradeParams, null );
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