using UnityEngine;
using UnityEngine.UI;

namespace MyLibrary {
    public class SetImageColorView : PropertyView {
        private Image mImage;
        public Image Image {
            get {
                if ( mImage == null ) {
                    mImage = GetComponent<Image>();
                }

                return mImage;
            }
        }

        public override void UpdateView() {
            Color textColor = GetValue<Color>();

            if ( Image != null ) {
                Image.color = textColor;
            }
        }
    }
}
