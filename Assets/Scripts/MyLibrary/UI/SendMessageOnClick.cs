using UnityEngine;

namespace MyLibrary {
    public class SendMessageOnClick : MonoBehaviour {
        public string Message;

        public void OnClick() {
            MyMessenger.Send( Message );
        }
    }
}