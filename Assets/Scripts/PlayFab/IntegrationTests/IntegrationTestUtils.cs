using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public static class IntegrationTestUtils {
        public const string TARGET_ID = "TargetID";
        public const string UPGRADE_ID = "UpgradeID";        
        public const string CHANGE = "Change";
        public const string CLASS = "Class";
        public const string SAVE_KEY = "SaveKey";       // used in params sent to cloud for test methods
        public const string DATA_ACCESS = "DataAccess"; // internal, read only, etc

        // constants on the server
        public const string ACCESS_INTERNAL = "Internal";
        public const string ACCESS_READ_ONLY = "ReadOnly";

        public static IEnumerator UpgradeTarget_NoRules( string i_targetID, string i_className ) {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[TARGET_ID] = i_targetID;
            testParams[CLASS] = i_className;

            BackendManager.Backend.MakeCloudCall( CloudTestMethods.testUpgrade.ToString(), testParams, null );

            yield return BackendManager.Backend.WaitUntilNotBusy();
        }

        public static IEnumerator TrainUnit( string i_unitID, int i_trainingLevelChange ) {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[TARGET_ID] = i_unitID;
            testParams[CLASS] = GenericDataLoader.UNITS;
            testParams[IdleFantasyBackend.CHANGE] = i_trainingLevelChange.ToString();

            BackendManager.Backend.MakeCloudCall( CloudTestMethods.testChangeTraining.ToString(), testParams, null );

            yield return BackendManager.Backend.WaitUntilNotBusy();
        }

        public static void SetReadOnlyData( string i_key, string i_value ) {
            SetSaveData( i_key, i_value, ACCESS_READ_ONLY );
        }

        public static void SetInternalData( string i_key, string i_value ) {
            SetSaveData( i_key, i_value, ACCESS_INTERNAL );
        }

        public static void SetSaveData( string i_key, string i_value, string i_access ) {
            Dictionary<string, string> setDataParams = new Dictionary<string, string>();
            setDataParams["Key"] = i_key;
            setDataParams["Value"] = i_value;
            setDataParams[DATA_ACCESS] = i_access;
            BackendManager.Backend.MakeCloudCall( CloudTestMethods.setSaveData.ToString(), setDataParams, null );
        }

        public static void SetPlayerCurrency( int i_amount ) {
            Dictionary<string, string> setCurrencyParams = new Dictionary<string, string>();
            setCurrencyParams["Type"] = VirtualCurrencies.GOLD;
            setCurrencyParams["Amount"] = i_amount.ToString();
            BackendManager.Backend.MakeCloudCall( CloudTestMethods.setPlayerCurrency.ToString(), setCurrencyParams, null );
        }
    }
}