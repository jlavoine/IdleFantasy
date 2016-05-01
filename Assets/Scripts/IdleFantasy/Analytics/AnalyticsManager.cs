using UnityEngine;
using MyLibrary;

#pragma warning disable 0219

namespace IdleFantasy {
    public class AnalyticsManager : MonoBehaviour {

        void Awake() {
            DontDestroyOnLoad( this );

            IMessageService messenger = new MyMessenger();
            PlayFabAnalytics playFab = new PlayFabAnalytics( messenger );
        }
    }
}