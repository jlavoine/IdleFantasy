
namespace IdleFantasy {
    public static class BackendConstants {
        #region Parameters
        public const string TARGET_ID = "TargetID";
        public const string UPGRADE_ID = "UpgradeID";
        public const string CHANGE = "Change";
        public const string CLASS = "Class";
        public const string SAVE_KEY = "SaveKey";       // used in params sent to cloud for test methods
        public const string DATA_ACCESS = "DataAccess"; // internal, read only, etc
        public const string KEY = "Key";
        public const string VALUE = "Value";
        public const string TYPE = "Type";
        public const string AMOUNT = "Amount";
        public const string POINTS = "Points";
        public const string PROGRESS = "Progress";
        public const string MISSION_TYPE = "MissionCategory";
        public const string MISSION_INDEX = "Index";
        public const string MISSION_PROPOSALS = "TaskProposals";
        public const string MAP_LEVEL = "MapLevel";
        public const string MAP_WORLD = "World";
        public const string MAP_SIZE = "MapSize";
        public const string MINIMUM = "Minimum";
        public const string INDEX = "Index";
        public const string MAP = "Map";
        #endregion

        #region Cloud Methods
        public const string INIT_UPGRADE = "initiateUpgrade";
        public const string INIT_TRAINER_PURCHASE = "initiateTrainerPurchase";
        public const string INIT_TRAINING_CHANGE = "initiateChangeInTraining";
        public const string ADD_POINTS_TO_UPGRADE = "addPointsToUpgrade";
        public const string ADD_PROGRESS_TO_UPGRADE = "addProgressToUpgrade";
        public const string GET_MISSIONS = "getTestMissions";
        public const string COMPLETE_MISSION = "initiateCompleteMission";
        public const string TRAVEL_TO = "initiateTravelTo";
        #endregion

        #region Map Piece Modifiers
        public const string COMBAT_WEIGHT = "CombatWeight";
        public const string COMBAT_MIN = "CombatMinimum";
        public const string EXPLORE_WEIGHT = "ExploreWeight";
        public const string EXPLORE_MIN = "ExploreMinimum";
        public const string MISC_WEIGHT = "MiscWeight";
        public const string MISC_MIN = "MiscMinimum";
        public const string BASE_GOLD_MOD = "BaseGoldReward";
        #endregion

        #region Player save data keys
        public const string BUILDING_PROGRESS = "BuildingsProgress";
        public const string GUILD_PROGRESS = "GuildsProgress";
        public const string UNIT_PROGRESS = "UnitsProgress";
        public const string WORLD_PROGRESS = "WorldsProgress";
        public const string TRAINER_PROGRESS = "TrainerSaveData";
        public const string MISSION_PROGRESS = "MissionProgress";
        public const string MAP_BASE = "Map_Base";
        public const string GAME_METRICS = "GameMetrics";
        #endregion

        #region Title data keys
        public const string UNIT_UNLOCKS = "UnitUnlockPlan";
        #endregion

        #region Worlds
        public const string WORLD_BASE = "Base";
        #endregion

        public const string DATA = "data";

        // constants on the server
        public const string ACCESS_INTERNAL = "Internal";
        public const string ACCESS_READ_ONLY = "ReadOnly";
    }
}
