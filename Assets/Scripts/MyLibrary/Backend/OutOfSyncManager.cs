/// <summary>
/// I'm making this generic, but it may not be.
/// It's this managers job to restart the client if
/// the backend becomes out of sync.
/// </summary>

using UnityEngine;

namespace MyLibrary {
    public class OutOfSyncManager {
        public OutOfSyncManager() {
            SubscribeForMessages();
        }

        public void Dispose() {
            UnsubscribeFromMessages();
        }

        private void SubscribeForMessages() {
            MyMessenger.AddListener( BackendMessages.BACKEND_OUT_OF_SYNC, RestartClient );
        }

        private void UnsubscribeFromMessages() {
            MyMessenger.RemoveListener( BackendMessages.BACKEND_OUT_OF_SYNC, RestartClient );
        }     

        private void RestartClient() {
            GameObject mainCanvas = GameObject.Find( "MainCanvas" );
            mainCanvas.InstantiateUI( "RestartClientPopup" );
        }
    }
}
