using UnityEngine;
using System;

namespace MyLibrary {
    public class InfoPopupTestHelper : MonoBehaviour {
        void Update() {
            if ( Input.GetKeyDown( KeyCode.P ) ) {
                QueueInfoPopup();
            }
        }

        private void QueueInfoPopup() {
            ViewModel model = new ViewModel();
            model.SetProperty( InfoPopupProperties.MAIN_TEXT, System.Guid.NewGuid().ToString() );

            MyMessenger.Send<string, ViewModel>( InfoPopupEvents.QUEUE, "StandardInfoPopup", model );
        }
    }
}
