using MyLibrary;
using MyLibrary.PlayFab;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy {
    public class IdleFantasyBackend : PlayFabBackend, IBackend {
        public static string INIT_UPGRADE = "initiateUpgrade";
        public static string INIT_TRAINER_PURCHASE = "initiateTrainerPurchase";
        public static string INIT_TRAINING_CHANGE = "initiateChangeInTraining";

        public static string TEST_SET_DATA = "setPlayerData";
        public static string TEST_SET_CURRENCY = "setPlayerCurrency";

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
            assignParams.Add( "Change", i_change.ToString() );
            assignParams.Add( "TargetID", i_unitID );

            MakeCloudCall( INIT_TRAINING_CHANGE, assignParams, null );
        }
    }
}