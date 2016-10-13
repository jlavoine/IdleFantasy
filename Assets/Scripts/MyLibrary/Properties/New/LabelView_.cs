using UnityEngine.UI;

namespace MyLibrary {
    public class LabelView_ : PropertyView {
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
            } else {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "No text element for LabelView: " + PropertyName, "UI" );
            }
        }
    }
}
