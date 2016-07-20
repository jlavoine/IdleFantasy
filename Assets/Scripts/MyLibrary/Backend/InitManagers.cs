using UnityEngine;
using MyLibrary;

#pragma warning disable 0219

namespace IdleFantasy {
    public class InitManagers : MonoBehaviour {

        void Awake() {
            DontDestroyOnLoad( this );

            MyLogger logger = new MyLogger();
            PlayFabAnalytics playFab = new PlayFabAnalytics();
            InfoPopupManager manager = new InfoPopupManager();
        }
    }
}
