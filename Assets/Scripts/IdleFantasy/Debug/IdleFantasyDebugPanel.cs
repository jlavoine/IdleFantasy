using IdleFantasy.PlayFab.IntegrationTests;
using MyLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace IdleFantasy {
    public class IdleFantasyDebugPanel : DebugPanel {

        protected override void AddDefaultCommands() {
            base.AddDefaultCommands();

            AddSetGoldCommand();
            AddResetAccountCommand();
            AddFastForwardCommand();
        }

        private void AddSetGoldCommand() {
            AddCommand( "set_gold", "set_gold <amount>", "Sets gold to a value.", ( result ) => {
                if ( result.Length == 2 ) {
                    int amount = 0;
                    int.TryParse( result[1], out amount );
                    StartCoroutine( SetGoldAction( amount ) );
                } else {
                    EchoImproperArgs();
                }               
            } );
        }

        private IEnumerator SetGoldAction( int i_amount ) {
            yield return IntegrationTestUtils.SetPlayerCurrencyAndWait( i_amount );
            IResourceInventory inventory = (IResourceInventory) PlayerManager.Data;
            inventory.SetResources( VirtualCurrencies.GOLD, i_amount );
        }

        private void AddResetAccountCommand() {
            AddCommand( "reset_account", "reset_account", "Completely resets this account.", ( result ) => {
                StartCoroutine( ResetAccountAction() );
            } );            
        }

        private IEnumerator ResetAccountAction() {
            yield return BackendManager.Backend.WaitForCloudCall( CloudTestMethods.deleteAllPlayerReadOnlyData.ToString(), PlayFabBackend.NULL_CLOUD_PARAMS, PlayFabBackend.NULL_CLOUD_CALLBACK );
            SceneManager.LoadScene( SceneList.LOGIN );
        }

        private void AddFastForwardCommand() {
            AddCommand( "fast_forward", "fast_forward <amount>", "Fast forwards time by <amount> milliseconds.", ( result ) => {
                if ( result.Length == 2 ) {
                    long amount = 0;
                    long.TryParse( result[1], out amount );
                    StartCoroutine( FastForwardAction( amount ) );
                }
                else {
                    EchoImproperArgs();
                }
            } );
        }

        private IEnumerator FastForwardAction( long i_time ) {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[BackendConstants.TIME] = i_time.ToString();

            BackendManager.Backend.MakeCloudCall( CloudTestMethods.testUpdateUnitCount.ToString(), testParams, null );
            yield return BackendManager.Backend.WaitUntilNotBusy();

            SceneManager.LoadScene( SceneList.LOGIN );
        }
    }
}
