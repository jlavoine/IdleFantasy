using TMPro;
using UnityEngine.UI;

namespace MyLibrary {
    public abstract class BaseLabelView : PropertyView {
        private TextMeshProUGUI mTextFieldPro;
        public TextMeshProUGUI TextFieldPro {
            get {
                if ( mTextFieldPro == null ) {
                    mTextFieldPro = GetComponent<TextMeshProUGUI>();
                }

                return mTextFieldPro;
            }
        }

        private Text mTextField;
        public Text TextField {
            get {
                if ( mTextField == null ) {
                    mTextField = GetComponent<Text>();
                }

                return mTextField;
            }
        }

        protected void SetText( string i_text ) {
            if ( TextFieldPro != null ) {
                TextFieldPro.text = i_text;
            } else if ( TextField != null ) {
                TextField.text = i_text;
            } else {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "No text element for LabelView: " + PropertyName, "UI" );
            }
        }
    }
}
