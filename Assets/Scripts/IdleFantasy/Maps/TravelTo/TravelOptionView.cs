using MyLibrary;

namespace IdleFantasy {
    public class TravelOptionView : GroupView {
        TravelOption mOption;

        public void Init( TravelOption i_option ) {
            mOption = i_option;

            SetModel( i_option.ViewModel );
        }

        public void OnClick() {
            TravelToOption();
        }

        private void TravelToOption() {            
            mOption.TravelToOption();
        }
    }
}
