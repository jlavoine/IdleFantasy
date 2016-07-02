using UnityEngine;

namespace MyLibrary {
    public class ClosableObject : MonoBehaviour {

        public void StartClose() {
            // TODO: Trigger close animation here, if applicable
            DestroyThis();
        }

        private void DestroyThis() {
            Destroy( gameObject );
        }
    }
}
