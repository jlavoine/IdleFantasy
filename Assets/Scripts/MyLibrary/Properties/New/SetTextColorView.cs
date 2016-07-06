using UnityEngine;
using UnityEngine.UI;

namespace MyLibrary {
    public class SetTextColorView : PropertyView {
        private Text mTextField;
        public Text TextField {
            get {
                if ( mTextField == null ) {
                    mTextField = GetComponent<Text>();
                }

                return mTextField;
            }
        }

        public override void UpdateView() {
            Color textColor = GetValue<Color>();
    
            if ( TextField != null ) {
                TextField.color = textColor;
            }
        }
    }
}
