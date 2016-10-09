using UnityEngine;

namespace MyLibrary {
    [RequireComponent( typeof( CanvasGroup ) )]
    public class SetCanvasVisibilityView : PropertyView {
        private CanvasGroup mCanvasGroup;
        public CanvasGroup CanvasGroup {
            get {
                if ( mCanvasGroup == null ) {
                    mCanvasGroup = GetComponent<CanvasGroup>();
                }

                return mCanvasGroup;
            }
        }

        public override void UpdateView() {
            SetCanvasGroupAlpha();
            SetCanvasInteractables();
        }

        private void SetCanvasGroupAlpha() {           
            CanvasGroup.alpha = GetValue<float>();
        }

        private void SetCanvasInteractables() {
            bool isInteractable = GetValue<float>() > 0;
            CanvasGroup.interactable = isInteractable;
        }
    }
}
