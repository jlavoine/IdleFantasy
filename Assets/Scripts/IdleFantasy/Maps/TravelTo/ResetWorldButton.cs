using UnityEngine;
using MyLibrary;

namespace IdleFantasy {
    public class ResetWorldButton : MonoBehaviour {
        public void OnClick() {
            SendWorldResetRequestToServer();
            SendTravelOptionSelectedMessage();
            ClosePopup();
        }

        private void SendWorldResetRequestToServer() {
            BackendManager.Backend.SendWorldResetRequest( BackendConstants.WORLD_BASE );
        }

        private void SendTravelOptionSelectedMessage() {
            EasyMessenger.Instance.Send( MapKeys.TRAVEL_TO_REQUEST, -1 );
        }

        private void ClosePopup() {
            ClosableObject.CloseViewForObject( gameObject );
        }
    }
}