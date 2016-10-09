using UnityEngine.Advertisements;

namespace MyLibrary {
    public class AdManager : IAdManager {
        private const string ANDROID_ID = "1157955";
        private const string IOS_ID = "1157956";
        private const string REWARD_VIDEO_ZONE = "video";
        public const string REWARD_AD_FINISHED_MESSAGE = "RewardAdFinished";

        private static IAdManager mInstance;
        public static IAdManager Instance {
            get {
                if ( mInstance == null ) {
                    mInstance = new AdManager();
                }

                return mInstance;
            }
            set {
                // unit tests only!
                mInstance = value;
            }
        }

        public AdManager() {
            Initialize();
        }

        private void Initialize() {
            #if UNITY_IOS
            Advertisement.Initialize( IOS_ID, true );
            #endif

            #if UNITY_ANDROID
            Advertisement.Initialize( ANDROID_ID, true );
            #endif
        }

        public bool IsAdReady() {
            #if UNITY_IOS || UNITY_ANDROID
            return Advertisement.IsReady();
            #else
            return true;
            #endif
        }

        public void RequestRewardAd() {
            #if UNITY_IOS || UNITY_ANDROID
            ShowOptions options = new ShowOptions();
            options.resultCallback = OnRewardAdFinishedWithShowResults;

            Advertisement.Show( REWARD_VIDEO_ZONE, options );
            #else
            OnRewardAdFinished( AdResults.Finished );
            #endif
        }

        #if UNITY_ANDROID || UNITY_IOS
        private void OnRewardAdFinishedWithShowResults( ShowResult i_result ) {
            // this is to get around the fact that ShowResult is not available in PC builds, so we created our own result enum
            AdResults adResult = (AdResults) i_result;
            OnRewardAdFinished( adResult );
        }
        #endif

        private void OnRewardAdFinished( AdResults i_result ) {
            EasyMessenger.Instance.Send( REWARD_AD_FINISHED_MESSAGE, i_result );            
        }
    }
}