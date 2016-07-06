using UnityEngine.UI;

namespace MyLibrary {
    public class SetInteractableView : PropertyView {
        private Button mInteractable;
        public Button Interactable {
            get {
                if ( mInteractable == null ) {
                    mInteractable = GetComponent<Button>();
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