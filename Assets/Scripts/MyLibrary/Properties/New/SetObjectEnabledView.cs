using System.Collections.Generic;
using UnityEngine;

namespace MyLibrary {
    public class SetObjectEnabledView : PropertyView {
        public List<GameObject> Objects;
        public bool CheckForValue;
        public bool ShouldEnable;        

        public override void UpdateView() {
            bool value = GetValue<bool>();
            bool enabledState = false;
            if ( value == CheckForValue ) {
                enabledState = ShouldEnable;
            } else {
                enabledState = !ShouldEnable;
            }

            SetObjectsEnabledState( enabledState );
        }

        private void SetObjectsEnabledState( bool i_enabled ) {
            foreach ( GameObject obj in Objects ) {
                obj.SetActive( i_enabled );
            }
        }
    }
}