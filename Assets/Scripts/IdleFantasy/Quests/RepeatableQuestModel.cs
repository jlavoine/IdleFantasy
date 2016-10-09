using MyLibrary;
using UnityEngine.Advertisements;

namespace IdleFantasy {
    public class RepeatableQuestModel  {
        public const string CURRENT_COMPLETED_COUNT_PROPERTY = "CurrentCount";
        public const string MAX_COMPLETED_COUNT_PROPERTY = "MaxCount";
        public const string WATCH_AD_PROPERTY = "TeaserText";
        public const string MISSION_VISIBLE_PROPERTY = "MissionVisibility";
        public const string AD_VISIBLE_PROPERTY = "AdPanelVisibility";
        public const string AD_INTERACTABLE_PROPERTY = "AdPanelInteractable";

        public const string AD_FINISHED_MESSAGE = "RepeatableAdVideoFinished";

        private ViewModel mModel;
        public ViewModel ViewModel { get { return mModel; } }

        private Mission mMission;
        public Mission Mission {
            get {
                if ( mMission == null ) {
                    mMission = new Mission( mProgress.GetMissionData() );
                }

                return mMission;
            }
        }

        private IRepeatableQuestProgress mProgress;
        public IRepeatableQuestProgress Progress { get { return mProgress; } }

        public RepeatableQuestModel( IRepeatableQuestProgress i_progress, IAdManager i_adManager ) {
            mModel = new ViewModel();
            mProgress = i_progress;

            SetUpModel( i_adManager );
            SubscribeToMessages();
        }

        public void Dispose() {
            UnsubscribeFromMessages();
        }

        private void SubscribeToMessages() {
            EasyMessenger.Instance.AddListener<AdResults>( AdManager.REWARD_AD_FINISHED_MESSAGE, OnRewardAdFinished );
        }

        private void UnsubscribeFromMessages() {
            EasyMessenger.Instance.RemoveListener<AdResults>( AdManager.REWARD_AD_FINISHED_MESSAGE, OnRewardAdFinished );
        }

        private void OnRewardAdFinished( AdResults i_result ) {
            switch ( i_result ) {
                case AdResults.Failed:
                case AdResults.Skipped:
                    PlayerFailedRewardAd();                    
                    break;
                case AdResults.Finished:
                    PlayerFinishedRewardAd();
                    break;
                default:
                    EasyLogger.Instance.Log( LogTypes.Warn, "Unhandled ad result in repeatable quest: " + i_result );
                    break;
            }
        }

        private void PlayerFailedRewardAd() {
            SetAdPanelTextProperty( false );
        }

        private void PlayerFinishedRewardAd() {
            EasyMessenger.Instance.Send( AD_FINISHED_MESSAGE );
            SetMissionVisibilityProperties();
            SetAdPanelVisibleProperty();
        }

        private void SetUpModel( IAdManager i_adManager ) {
            SetMissionVisibilityProperties();
            SetAdPanelVisibleProperty();          
            SetCompletedCountProperties();
            SetAdPanelTextProperty( i_adManager.IsAdReady() );
            SetAdPanelInteractableProperty( i_adManager.IsAdReady() );
        }

        private void SetMissionVisibilityProperties() {
            Mission.ViewModel.SetProperty( MISSION_VISIBLE_PROPERTY, mProgress.IsQuestAvailable() && !mProgress.IsDone() ? 1f : 0f );
        }

        private void SetCompletedCountProperties() {
            ViewModel.SetProperty( CURRENT_COMPLETED_COUNT_PROPERTY, mProgress.GetCompletedCount() );
            ViewModel.SetProperty( MAX_COMPLETED_COUNT_PROPERTY, Constants.GetConstant<int>( ConstantKeys.MAX_REPEATABLE_QUESTS ) );
        }

        private void SetAdPanelVisibleProperty() {
            ViewModel.SetProperty( AD_VISIBLE_PROPERTY, mProgress.IsQuestAvailable() && !mProgress.IsDone() ? 0f : 1f );
        }

        private void SetAdPanelTextProperty( bool i_isAdAvailable ) {
            string textKey = i_isAdAvailable && !mProgress.IsDone() ? StringKeys.REPEATABLE_QUEST_AD_AVAILABLE : StringKeys.REPEATABLE_QUEST_AD_UNAVAILABLE;
            ViewModel.SetProperty( WATCH_AD_PROPERTY, StringTableManager.Get( textKey ) );
        }

        private void SetAdPanelInteractableProperty( bool i_isAdReady ) {
            ViewModel.SetProperty( AD_INTERACTABLE_PROPERTY, i_isAdReady && !mProgress.IsQuestAvailable() && !mProgress.IsDone() );
        }
    }
}
