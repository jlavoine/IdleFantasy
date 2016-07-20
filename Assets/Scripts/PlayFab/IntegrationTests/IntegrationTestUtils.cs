using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public static class IntegrationTestUtils {
        public static IEnumerator UpgradeTarget_NoRules( string i_targetID, string i_className ) {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[BackendConstants.TARGET_ID] = i_targetID;
            testParams[BackendConstants.CLASS] = i_className;

            BackendManager.Backend.MakeCloudCall( CloudTestMethods.testUpgrade.ToString(), testParams, null );

            yield return BackendManager.Backend.WaitUntilNotBusy();
        }

        public static IEnumerator TrainUnit( string i_unitID, int i_trainingLevelChange ) {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[BackendConstants.TARGET_ID] = i_unitID;
            testParams[BackendConstants.CLASS] = GenericDataLoader.UNITS;
            testParams[BackendConstants.CHANGE] = i_trainingLevelChange.ToString();

            BackendManager.Backend.MakeCloudCall( CloudTestMethods.testChangeTraining.ToString(), testParams, null );

            yield return BackendManager.Backend.WaitUntilNotBusy();
        }

        public static void SetReadOnlyData( string i_key, string i_value ) {
            SetSaveData( i_key, i_value, BackendConstants.ACCESS_READ_ONLY );
        }

        public static void SetInternalData( string i_key, string i_value ) {
            SetSaveData( i_key, i_value, BackendConstants.ACCESS_INTERNAL );
        }

        public static void SetSaveData( string i_key, string i_value, string i_access ) {
            Dictionary<string, string> setDataParams = new Dictionary<string, string>();
            setDataParams[BackendConstants.KEY] = i_key;
            setDataParams[BackendConstants.VALUE] = i_value;
            setDataParams[BackendConstants.DATA_ACCESS] = i_access;
            BackendManager.Backend.MakeCloudCall( CloudTestMethods.setSaveData.ToString(), setDataParams, null );
        }

        public static void SetPlayerCurrency( int i_amount ) {
            Dictionary<string, string> setCurrencyParams = new Dictionary<string, string>();
            setCurrencyParams[BackendConstants.TYPE] = VirtualCurrencies.GOLD;
            setCurrencyParams[BackendConstants.AMOUNT] = i_amount.ToString();
            BackendManager.Backend.MakeCloudCall( CloudTestMethods.setPlayerCurrency.ToString(), setCurrencyParams, null );
        }
    }
}