using UnityEngine;

namespace MyLibrary {
    public class OpenGenericPopup : MonoBehaviour {
        public GameObject PopupPrefab;

        public void OnClick() {
            CreatePopup();
        }

        private void CreatePopup() {
            gameObject.InstantiateUI( PopupPrefab );
        }
    }
}
