using UnityEngine;

namespace MyLibrary {
    public class GroupView : MonoBehaviour {

        protected virtual void OnDestroy() { }

        public void SetModel( ViewModel i_model ) {
            SetModelForAllChildrenViews( i_model );
        }

        private void SetModelForAllChildrenViews( ViewModel i_model ) {
            PropertyView[] viewsInGroup = GetComponentsInChildren<PropertyView>();
            foreach ( PropertyView view in viewsInGroup ) {
                SetModelOnChildView( i_model, view );                
            }
        }

        private void SetModelOnChildView( ViewModel i_model, PropertyView i_view ) {
            GroupView parentGroupView = i_view.gameObject.GetComponentInParent<GroupView>();
            if ( this == parentGroupView ) {
                i_view.SetModel( i_model );
            }
        }
    }
}