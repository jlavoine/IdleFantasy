/// <summary>
/// I'm making this generic, but it may not be.
/// It's this managers job to restart the client if
/// the backend becomes out of sync.
/// </summary>

using UnityEngine;
using System.Collections.Generic;

namespace MyLibrary {
    public class OutOfSyncManager {
        public OutOfSyncManager() {
            SubscribeForMessages();
        }

        public void Dispose() {
            UnsubscribeFromMessages();
        }

        private void SubscribeForMessages() {
            MyMessenger.AddListener<string>( BackendMessages.BACKEND_OUT_OF_SYNC, OnBackendOutOfSync );
        }

        private void UnsubscribeFromMessages() {
            MyMessenger.RemoveListener<string>( BackendMessages.BACKEND_OUT_OF_SYNC, OnBackendOutOfSync );
        }     

        private void OnBackendOutOfSync( string i_reason ) {
            SendOutOfSyncAnaltyic( i_reason );
            RestartClient();
        }

        private void SendOutOfSyncAnaltyic( string i_reason ) {
            Dictionary<string, object> eventData = new Dictionary<string, object>();
            eventData.Add( LibraryAnalyticEvents.REASON, i_reason );

            MyMessenger.Send<string, IDictionary<string, object>>( LibraryAnalyticEvents.SEND_ANALYTIC_EVENT, LibraryAnalyticEvents.CLIENT_LOST_SYNC, eventData );
        }

        private void RestartClient() {
            GameObject mainCanvas = GameObject.FindGameObjectWithTag( "MainCanvas" );
            mainCanvas.InstantiateUI( "RestartClientPopup" );
        }
    }
}
