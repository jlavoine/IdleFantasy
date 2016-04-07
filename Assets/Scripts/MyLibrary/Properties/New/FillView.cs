using UnityEngine.UI;

namespace MyLibrary {
    public class FillView : PropertyView {
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
            if ( Image != null ) {
                float fillPercent = GetValue<float>();
                Image.fillAmount = fillPercent;
            }
        }
    }
}