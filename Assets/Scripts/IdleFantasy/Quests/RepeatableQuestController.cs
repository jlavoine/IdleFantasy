using UnityEngine;
using MyLibrary;

namespace IdleFantasy {
    public class RepeatableQuestController : MonoBehaviour {
        public void ShowRewardAd() {
            AdManager.Instance.RequestRewardAd();
        }
    }
}
