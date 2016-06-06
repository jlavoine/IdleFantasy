using System.Collections;
using System.Collections.Generic;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public static class IntegrationTestUtils {
        public const string TARGET_ID = "TargetID";
        public const string CLASS = "Class";
        public const string SAVE_KEY = "SaveKey";    // used in params sent to cloud for test methods

        public static IEnumerator UpgradeTarget_NoRules( string i_targetID, string i_className ) {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[TARGET_ID] = i_targetID;
            testParams[CLASS] = i_className;

            BackendManager.Backend.MakeCloudCall( IdleFantasyBackend.TEST_UPGRADE, testParams, null );

            yield return BackendManager.Backend.WaitUntilNotBusy();
        }

        public static IEnumerator TrainUnit( string i_unitID, int i_trainingLevelChange ) {
            Dictionary<string, string> testParams = new Dictionary<string, string>();
            testParams[TARGET_ID] = i_unitID;
            testParams[CLASS] = GenericDataLoader.UNITS;
            testParams[IdleFantasyBackend.CHANGE] = i_trainingLevelChange.ToString();

            BackendManager.Backend.MakeCloudCall( IdleFantasyBackend.TEST_CHANGE_TRAINING, testParams, null );

            yield return BackendManager.Backend.WaitUntilNotBusy();
        }

        public static void SetPlayerData( string i_key, string i_value ) {
            Dictionary<string, string> setDataParams = new Dictionary<string, string>();
            setDataParams["Key"] = i_key;
            setDataParams["Value"] = i_value;
            BackendManager.Backend.MakeCloudCall( IdleFantasyBackend.TEST_SET_DATA, setDataParams, null );
        }

        public static void SetInternalData( string i_key, string i_value ) {
            Dictionary<string, string> setDataParams = new Dictionary<string, string>();
            setDataParams[SAVE_KEY] = i_key;
            setDataParams["Value"] = i_value;
            BackendManager.Backend.MakeCloudCall( IdleFantasyBackend.TEST_SET_INTERNAL_DATA, setDataParams, null );
        }

        public static void SetPlayerCurrency( int i_amount ) {
            Dictionary<string, string> setCurrencyParams = new Dictionary<string, string>();
            setCurrencyParams["Type"] = VirtualCurrencies.GOLD;
            setCurrencyParams["Amount"] = i_amount.ToString();
            BackendManager.Backend.MakeCloudCall( IdleFantasyBackend.TEST_SET_CURRENCY, setCurrencyParams, null );
        }
    }
}