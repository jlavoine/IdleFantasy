using UnityEngine;

namespace MyLibrary {
    public class GroupView : MonoBehaviour {

        public void SetModel( ViewModel i_model ) {
            PropertyView[] viewsInGroup = GetComponentsInChildren<PropertyView>();
            foreach( PropertyView view in viewsInGroup ) {
                view.SetModel( i_model );
            }
        }
    }
}