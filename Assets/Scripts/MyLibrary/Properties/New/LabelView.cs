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
            string label = mModel.GetPropertyValue<string>( PropertyName );

            if ( TextField != null ) {
                TextField.text = label;
            }
        }
    }
}
