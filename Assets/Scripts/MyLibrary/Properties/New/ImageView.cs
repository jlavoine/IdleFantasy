using UnityEngine;
using UnityEngine.UI;

namespace MyLibrary {
    public class ImageView : PropertyView {
        public string Prefix;

        private Image mImage;
        public Image ImageField {
            get {
                if ( mImage == null ) {
                    mImage = GetComponent<Image>();
                }

                return mImage;
            }
        }

        public override void UpdateView() {
            string imageKey = Prefix + GetValue<string>();
            Sprite sprite = SpriteExtensions.GetSpriteFromResource( imageKey );

            if ( ImageField != null ) {
                ImageField.sprite = sprite;
            }
        }
    }
}