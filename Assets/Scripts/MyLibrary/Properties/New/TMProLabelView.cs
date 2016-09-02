using TMPro;

namespace MyLibrary {
    public class TMProLabelView : PropertyView {
        private TextMeshProUGUI mTextField;
        public TextMeshProUGUI TextField {
            get {
                if ( mTextField == null ) {
                    mTextField = GetComponent<TextMeshProUGUI>();
                }

                return mTextField;
            }
        }

        public override void UpdateView() {
            object propertyValue = GetValue<object>();
            string label = propertyValue.ToString();

            if ( TextField != null ) {
                TextField.SetText( label );
            }
            else {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "No text element for TMProLabelView: " + PropertyName, "UI" );
            }
        }
    }
}
