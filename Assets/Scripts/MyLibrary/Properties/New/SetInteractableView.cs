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

        public bool TargetBoolValue = true;

        public override void UpdateView() {
            bool state = GetValue<bool>();

            if ( Interactable != null ) {
                Interactable.interactable = state == TargetBoolValue; ;
            } else {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "No interactable element for SetInteractableView: " + PropertyName, "UI" );
            }
        }
    }
}