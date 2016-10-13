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

        public static void CloseViewForObject( GameObject i_object ) {
            ClosableObject closable = i_object.GetComponentInParent<ClosableObject>();
            if ( closable ) {
                closable.StartClose();
            }
            else {
                EasyLogger.Instance.Log( LogTypes.Error, "Went to close " + i_object.name + " but it wasn't closable.", "UI" );
            }
        }
    }
}
