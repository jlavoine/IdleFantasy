using UnityEngine;
using MyLibrary;

namespace IdleFantasy {
    public class AnalyticsManager : MonoBehaviour {

        void Awake() {
            DontDestroyOnLoad( this );

            IMessageService messenger = new MyMessenger();
            PlayFabAnalytics playFab = new PlayFabAnalytics( messenger );
        }
    }
}