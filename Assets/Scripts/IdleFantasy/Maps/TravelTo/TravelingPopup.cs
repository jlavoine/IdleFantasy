using UnityEngine;
using MyLibrary;

namespace IdleFantasy {
    public class TravelingPopup : MonoBehaviour {

        public void Start() {
            SubscribeToMessages();
        }

        public void OnDestroy() {
            UnsubscribeFromMessages();
        }

        private void SubscribeToMessages() {
            MyMessenger.AddListener( MapKeys.TRAVEL_TO_SUCCESS, OnTravelSuccess );
        }

        private void UnsubscribeFromMessages() {
            MyMessenger.RemoveListener( MapKeys.TRAVEL_TO_SUCCESS, OnTravelSuccess );
        }

        private void OnTravelSuccess() {
            Destroy( gameObject );
        }
    }
}
