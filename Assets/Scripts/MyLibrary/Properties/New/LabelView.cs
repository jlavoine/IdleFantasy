using UnityEngine.UI;

namespace MyLibrary {
    public class LabelView : PropertyView {
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
            object propertyValue = GetValue<object>();
            string label = propertyValue.ToString();

            if ( TextField != null ) {
                TextField.text = label;
            }
        }
    }
}
