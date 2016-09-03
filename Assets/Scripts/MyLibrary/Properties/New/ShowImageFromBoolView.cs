using UnityEngine.UI;

namespace MyLibrary {
    public class ShowImageFromBoolView : PropertyView {
        private Image mImage;
        public Image Image {
            get {
                if ( mImage == null ) {
                    mImage = GetComponent<Image>();
                }

                return mImage;
            }
        }

        public bool ShowOnTarget;

        public override void UpdateView() {
            bool propertyValue = GetValue<bool>();

            if ( Image != null ) {
                bool show = propertyValue == ShowOnTarget;
                Image.enabled = show;
            }
            else {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "No image element for ShowImageFromBool: " + PropertyName, "UI" );
            }
        }
    }
}
