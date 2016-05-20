using MyLibrary;
using MyLibrary.PlayFab;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IdleFantasy {
    public class IdleFantasyBackend : PlayFabBackend, IBackend {

        public IdleFantasyBackend( IMessageService i_messenger ) : base(i_messenger) {
            mMessenger = i_messenger;
        }

        public void MakeUpgradeCall( string i_className, string i_targetID, string i_upgradeID ) {
            StartRequest( "Making upgrade call" );

            Dictionary<string, string> upgradeParams = new Dictionary<string, string>();
            upgradeParams.Add( "Class", i_className );
            upgradeParams.Add( "TargetID", i_targetID );
            upgradeParams.Add( "UpgradeID", i_upgradeID );

            MakeCloudCall( "initiateUpgrade", upgradeParams, null );
        }
    }
}