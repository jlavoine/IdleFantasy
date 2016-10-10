using System;
using System.Collections;
using System.Collections.Generic;
using MyLibrary;
using Newtonsoft.Json;

namespace IdleFantasy {
    public class PlayerData : IPlayerData, IResourceInventory {
        public const string PROGRESS_KEY = "Progress";
        public const string TRAINER_SAVE_DATA = "TrainerSaveData";
        public const string INVENTORY_CHANGED_EVENT = "InventoryChanged";

        public Dictionary<string, int> UnitTrainingLevels;

        private Hashtable mPlayerProgress = new Hashtable();
        public Dictionary<string, UnitProgress> UnitProgress { get { return (Dictionary<string, UnitProgress>) mPlayerProgress[GenericDataLoader.UNITS]; } }
        public Dictionary<string, BuildingProgress> BuildingProgress { get { return (Dictionary<string, BuildingProgress>) mPlayerProgress[GenericDataLoader.BUILDINGS]; } }
        public Dictionary<string, GuildProgress> GuildProgress { get { return (Dictionary<string, GuildProgress>) mPlayerProgress[GenericDataLoader.GUILDS]; } }

        private List<Guild> mGuilds = new List<Guild>();
        public List<Guild> Guilds { get { return mGuilds; } }

        private List<Building> mBuildings = new List<Building>();
        public List<Building> Buildings { get { return mBuildings; } }

        public Dictionary<string, MapData> mMaps = new Dictionary<string, MapData>();
        public Dictionary<string, MapData> Maps { get { return mMaps; } }

        public Dictionary<string, WorldMissionProgress> mMissionProgress = new Dictionary<string, WorldMissionProgress>();
        public Dictionary<string, WorldMissionProgress> MissionProgress { get { return mMissionProgress; } }

        public Dictionary<string, RepeatableQuestProgress> mRepeatableQuestProgress = new Dictionary<string, RepeatableQuestProgress>();
        public Dictionary<string, RepeatableQuestProgress> RepeatableQuestProgress { get { return mRepeatableQuestProgress; } }

        private TrainerSaveData mTrainerSaveData;
        private ITrainerManager mTrainerManager;
        public ITrainerManager TrainerManager { get { return mTrainerManager; } }

        private UnitUnlockPlanData mUnitUnlockPlan;
        public UnitUnlockPlanData UnitUnlockPlan { get { return mUnitUnlockPlan; } }

        private IGameMetrics mGameMetrics;
        public IGameMetrics GameMetrics { get { return mGameMetrics; } }

        private Dictionary<string, int> mInventory = new Dictionary<string, int>();

        private ViewModel mModel;

        private IBasicBackend mBackend;

        public void Init( IBasicBackend i_backend ) {
            SubscribeToMessages();

            mBackend = i_backend;
            mModel = new ViewModel();

            DownloadAllProgressData();
            DownloadTrainerData();
            DownloadCurrencyData();
            DownloadMapData();
            DownloadMissionProgress();
            DownloadUnlockPlan();
            DownloadGameMetrics();
            DownloadRepeatableQuestProgress();
        }

        private void SubscribeToMessages() {
            EasyMessenger.Instance.AddListener<MissionData>( MissionKeys.MISSION_COMPLETED, OnMissionClientComplete );
            EasyMessenger.Instance.AddListener<MissionData>( IdleFantasyBackend.MISSION_COMPLETED_ON_SERVER_MESSAGE, OnMissionServerComplete );
            EasyMessenger.Instance.AddListener<string>( Tutorial.TUTORIAL_FINISHED, OnTutorialFinished );
            EasyMessenger.Instance.AddListener( RepeatableQuestModel.AD_FINISHED_MESSAGE, OnRepeatableQuestAdFinished );
        }

        public void Dispose() {
            UnsubscribeFromMessages();
        }

        private void UnsubscribeFromMessages() {
            EasyMessenger.Instance.RemoveListener<MissionData>( MissionKeys.MISSION_COMPLETED, OnMissionClientComplete );
            EasyMessenger.Instance.RemoveListener<MissionData>( IdleFantasyBackend.MISSION_COMPLETED_ON_SERVER_MESSAGE, OnMissionServerComplete );
            EasyMessenger.Instance.RemoveListener<string>( Tutorial.TUTORIAL_FINISHED, OnTutorialFinished );
            EasyMessenger.Instance.RemoveListener( RepeatableQuestModel.AD_FINISHED_MESSAGE, OnRepeatableQuestAdFinished );
        }

        private void OnMissionClientComplete( MissionData i_missionData ) {            
            ApplyMissionRewards( i_missionData );        

            if ( i_missionData.MissionCategory == BackendConstants.MISSION_TYPE_MAP ) {
                IncrementMetric( GameMetricsList.TOTAL_MISSIONS_DONE );
                UpdateMissionProgress( i_missionData.MissionWorld, i_missionData.Index );
                CheckForUnitUnlock();
            } else if ( i_missionData.MissionCategory == BackendConstants.MISSION_TYPE_REPEATABLE_QUEST ) {
                SetRepeatableQuestFinished( BackendConstants.WORLD_BASE );
            }
        }

        private void SetRepeatableQuestFinished( string i_world ) {
            IRepeatableQuestProgress progress = GetRepeatableQuestForWorld( i_world );
            progress.SetMissionFinished();
        }

        private void OnMissionServerComplete( MissionData i_missionData ) {
            if ( i_missionData.MissionCategory == BackendConstants.MISSION_TYPE_REPEATABLE_QUEST ) {
                DownloadRepeatableQuestProgress();
            }
        }

        private void OnRepeatableQuestAdFinished() {
            RepeatableQuestProgress[BackendConstants.WORLD_BASE].CurrentlyAvailable = true;

            SendRepeatableQuestAdFinishedToServer( BackendConstants.WORLD_BASE );
        }

        private void SendRepeatableQuestAdFinishedToServer( string i_world ) {
            Dictionary<string, string> cloudParams = new Dictionary<string, string>();
            cloudParams.Add( BackendConstants.MAP_WORLD, i_world );

            BackendManager.Backend.MakeCloudCall( BackendConstants.WATCHED_REPEATABLE_QUEST_AD, cloudParams, null );
        }

        private void OnTutorialFinished( string i_tutorialName ) {
            IncrementMetric( i_tutorialName );

            SendTutorialCompleteMessageToServer( i_tutorialName );
        }

        private void SendTutorialCompleteMessageToServer( string i_tutorialName ) {
            Dictionary<string, string> cloudParams = new Dictionary<string, string>();
            cloudParams.Add( BackendConstants.VALUE, i_tutorialName );

            BackendManager.Backend.MakeCloudCall( BackendConstants.TUTORIAL_COMPLETE, cloudParams, null );
        }

        private void IncrementMetric( string i_metric ) {
            GameMetrics.IncrementMetric( i_metric );
        }

        private void ApplyMissionRewards( MissionData i_missionData ) {
            GainResources( VirtualCurrencies.GOLD, i_missionData.GoldReward );
        }

        private void UpdateMissionProgress( string i_missionWorld, int i_missionIndex ) {
            WorldMissionProgress missionProgress = MissionProgress[i_missionWorld];
            missionProgress.Missions[i_missionIndex].Completed = true;
        }

        private void CheckForUnitUnlock() {
            int totalMissionsCompleted = GameMetrics.GetMetric( GameMetricsList.TOTAL_MISSIONS_DONE );

            if ( UnitUnlockPlan.Unlocks.ContainsKey( totalMissionsCompleted ) ) {
                UnlockUnit( UnitUnlockPlan.Unlocks[totalMissionsCompleted] );
            }
        }

        private void UnlockUnit( UnitUnlockData i_unlock ) {
            ShowUnlockPopup( i_unlock.UnitID );
            UpdateUnitDataForUnlock( i_unlock.UnitID );
            SendUnitUnlockEvent( i_unlock.UnitID );
        }

        private void ShowUnlockPopup( string i_unitID ) {
            string unlockText = StringTableManager.Get( StringKeys.UNLOCK_TEXT );
            unlockText = DrsStringUtils.Replace( unlockText, StringKeys.CLASS_KEY, UnitKeys.GetName( i_unitID ) );

            ViewModel model = new ViewModel();
            model.SetProperty( InfoPopupProperties.MAIN_IMAGE, UnitKeys.GetIconKey( i_unitID ) );
            model.SetProperty( InfoPopupProperties.MAIN_TEXT, unlockText );

            MyMessenger.Send<string, ViewModel>( InfoPopupEvents.QUEUE, InfoPopupProperties.STANDARD_POPUP, model );
        }

        private void UpdateUnitDataForUnlock( string i_unitID ) {
            foreach ( Building building in Buildings ) {
                if ( building.Unit.GetID() == i_unitID ) {
                    building.Level.Upgrade();
                    building.Unit.Level.Upgrade();
                }
            }
        }

        private void SendUnitUnlockEvent( string i_unitID ) {
            MyMessenger.Send<string>( UnitKeys.UNLOCK_EVENT, i_unitID );
        }

        private void DownloadUnlockPlan() {
            mBackend.GetTitleData( BackendConstants.UNIT_UNLOCKS, ( jsonData ) => {
                mUnitUnlockPlan = JsonConvert.DeserializeObject<UnitUnlockPlanData>( jsonData );
            } );
        }

        private void DownloadGameMetrics() {
            mBackend.GetPlayerData( BackendConstants.GAME_METRICS, ( jsonData ) => {
                mGameMetrics = JsonConvert.DeserializeObject<GameMetrics>( jsonData );
            } );
        }

        private void DownloadRepeatableQuestProgress() {
            mBackend.GetPlayerData( BackendConstants.REPEATABLE_QUEST_PROGRESS, (jsonData) => {
                mRepeatableQuestProgress = JsonConvert.DeserializeObject<Dictionary<string, RepeatableQuestProgress>>( jsonData );
            });
        }

        private void DownloadMapData() {
            mBackend.GetPlayerData( BackendConstants.MAP_BASE, ( jsonData ) => {
                SetMapData( JsonConvert.DeserializeObject<MapData>( jsonData ) );
            } );
        }

        public void SetMapData( MapData i_mapData ) {
            string world = i_mapData.World;
            mMaps[world] = i_mapData;
        }

        private void DownloadMissionProgress() {
            mBackend.GetPlayerDataDeserialized<Dictionary<string, WorldMissionProgress>>( BackendConstants.MISSION_PROGRESS, ( progress ) => {
                mMissionProgress = progress;
            } );
        }

        public void SetMissionProgressForWorld( string i_world, WorldMissionProgress i_progress ) {
            mMissionProgress[i_world] = i_progress;
        }

        private void DownloadTrainerData() {
            mBackend.GetPlayerData( TRAINER_SAVE_DATA, ( jsonData ) => {
                mTrainerSaveData = JsonConvert.DeserializeObject<TrainerSaveData>( jsonData );
            } );
        }

        private void DownloadCurrencyData() {
            mBackend.GetVirtualCurrency( VirtualCurrencies.GOLD, ( numGold ) => {
                Gold = numGold;
            } );
        }
        private void DownloadAllProgressData() {
            DownloadProgressDataForKey<UnitProgress>( GenericDataLoader.UNITS );
            DownloadProgressDataForKey<BuildingProgress>( GenericDataLoader.BUILDINGS );
            DownloadProgressDataForKey<GuildProgress>( GenericDataLoader.GUILDS, AddGuilds );
        }

        public void AddDataStructures() {
            AddBuildings();
        }

        private void AddGuilds() {
            foreach ( KeyValuePair<string, GuildProgress> kvp in GuildProgress ) {
                Guilds.Add( new Guild( kvp.Value ) );
            }
        }

        private void AddBuildings() {
            foreach ( KeyValuePair<string, BuildingProgress> kvp in BuildingProgress ) {
                BuildingData buildingData = GenericDataLoader.GetData<BuildingData>( kvp.Value.ID );
                UnitProgress unitProgressForBuilding = UnitProgress[buildingData.Unit];
                Buildings.Add( new Building( kvp.Value, unitProgressForBuilding ) );
            }
        }

        private void DownloadProgressDataForKey<T>( string i_key, Callback i_doneDownloadingCallback = null ) where T : ProgressBase {
            string dataKey = i_key + PROGRESS_KEY;
            mBackend.GetPlayerData( dataKey, ( jsonData ) => {
                Dictionary<string, T> allProgressData = JsonConvert.DeserializeObject<Dictionary<string, T>>( jsonData );

                SetIDsOnProgressData( allProgressData );

                mPlayerProgress[i_key] = allProgressData;

                if ( i_doneDownloadingCallback != null ) {
                    i_doneDownloadingCallback();
                }
            } );
        }

        // This method exists to set IDs on all progress data, because the ID isn't inside the JSON itself
        private void SetIDsOnProgressData<T>( Dictionary<string, T> i_progressData ) where T : ProgressBase {
            foreach ( KeyValuePair<string, T> kvp in i_progressData ) {
                kvp.Value.ID = kvp.Key;
            }
        }

        public void CreateManagers() {
            mTrainerManager = new TrainerManager( mModel, mTrainerSaveData, UnitProgress );
        }

        public int Gold {
            get { return mModel.GetPropertyValue<int>( VirtualCurrencies.GOLD ); }
            private set {
                mModel.SetProperty( VirtualCurrencies.GOLD, value );

                if ( !mInventory.ContainsKey( VirtualCurrencies.GOLD ) ) {
                    mInventory.Add( VirtualCurrencies.GOLD, value );
                }
                else {
                    mInventory[VirtualCurrencies.GOLD] = value;
                }
            }
        }

        public ViewModel GetViewModel() {
            return mModel;
        }

        public int GetResourceCount( string i_resource ) {
            if ( mInventory.ContainsKey( i_resource ) ) {
                return mInventory[i_resource];
            }
            else {
                mInventory[i_resource] = 0;
                return 0;
            }
        }

        public bool HasEnoughResources( string i_resource, int i_count ) {
            int amountOfResource = GetResourceCount( i_resource );
            bool hasEnough = amountOfResource >= i_count;
            return hasEnough;
        }

        public void SpendResources( string i_resource, int i_count ) {
            int amountOfResource = GetResourceCount( i_resource );
            int remainingValue = Math.Max( amountOfResource - i_count, 0 );
            mInventory[i_resource] = remainingValue;

            UpdateInventoryData();
        }

        public void GainResources( string i_resource, int i_value ) {
            mInventory[i_resource] += i_value;
            UpdateInventoryData();
        }

        public void SetResources( string i_resource, int i_amount ) {
            mInventory[i_resource] = i_amount;
            UpdateInventoryData();
        }

        public void UpdateInventoryData() {
            foreach ( KeyValuePair<string, int> inventoryItem in mInventory ) {
                mModel.SetProperty( inventoryItem.Key, inventoryItem.Value );
                EasyMessenger.Instance.Send( INVENTORY_CHANGED_EVENT, inventoryItem.Key, inventoryItem.Value );
            }
        }

        public IWorldMissionProgress GetMissionProgressForWorld( string i_world ) {
            if ( MissionProgress.ContainsKey( i_world ) ) {
                return MissionProgress[i_world];
            } else {
                EasyLogger.Instance.Log( LogTypes.Fatal, "No mission progress for world: " + i_world, "" );
                return new WorldMissionProgress();
            }
        }

        public IRepeatableQuestProgress GetRepeatableQuestForWorld( string i_world ) {
            if ( RepeatableQuestProgress.ContainsKey( i_world ) ) {
                return RepeatableQuestProgress[i_world];
            } else {
                EasyLogger.Instance.Log( LogTypes.Fatal, "No repeatable quest progress for world: " + i_world, "" );
                return new RepeatableQuestProgress();
            }
        }

        public void PlayerTraveledToNewArea( Dictionary<string, string> i_travelData ) {
            MapData map = JsonConvert.DeserializeObject<MapData>( i_travelData[BackendConstants.MAP] );
            WorldMissionProgress progress = JsonConvert.DeserializeObject<WorldMissionProgress>( i_travelData[BackendConstants.MISSION_PROGRESS] );

            SendMapCompletedAnalytic( Maps[map.World] );
            SetMapData( map );
            SetMissionProgressForWorld( map.World, progress );
            SendTravelSuccessMessage();
        }

        private void SendMapCompletedAnalytic( MapData i_map ) {
            Dictionary<string, object> analyticParams = new Dictionary<string, object>();
            analyticParams.Add( Analytics.AREAS_COMPLETE, GetAreasCompletedForMap( i_map ) );
            analyticParams.Add( Analytics.MAP_LEVEL, i_map.MapLevel );
            analyticParams.Add( Analytics.MAP_WORLD, i_map.World );

            MyMessenger.Send<string, IDictionary<string, object>>( LibraryAnalyticEvents.SEND_ANALYTIC_EVENT, Analytics.MAP_COMPLETE_EVENT, analyticParams );
        }

        public int GetAreasCompletedForMap( MapData i_map ) {
            WorldMissionProgress progress = MissionProgress[i_map.World];
            return progress.GetCompletedMissionCount();
        }

        private void SendTravelSuccessMessage() {            
            MyMessenger.Send( MapKeys.TRAVEL_TO_SUCCESS );
        }
    }
}