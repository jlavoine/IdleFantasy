using UnityEngine;
using MyLibrary;

#pragma warning disable 0219

namespace IdleFantasy {
    public class InitManagers : MonoBehaviour {

        void Awake() {
            DontDestroyOnLoad( this );

            MyLogger logger = new MyLogger();
            UnityAnalyticsManager analytics = new UnityAnalyticsManager( new MyUnityAnalytics() );
            InfoPopupManager popupManager = new InfoPopupManager();
            OutOfSyncManager outOfSyncManager = new OutOfSyncManager();
        }
    }
}
