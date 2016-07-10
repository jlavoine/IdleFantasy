using UnityEngine.UI;

namespace MyLibrary {
    public class SetInteractableView : PropertyView {
        private Selectable mInteractable;
        public Selectable Interactable {
            get {
                if ( mInteractable == null ) {
                    mInteractable = GetComponent<Selectable>();
                }

                return mInteractable;
            }
        }

        public override void UpdateView() {
            bool state = GetValue<bool>();

            if ( Interactable != null ) {
                Interactable.interactable = state;
            }
        }
    }
}