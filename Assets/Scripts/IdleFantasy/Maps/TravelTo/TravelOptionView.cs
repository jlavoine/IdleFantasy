using MyLibrary;

namespace IdleFantasy {
    public class TravelOptionView : GroupView {

        public void Init( TravelOption i_option ) {
            SetModel( i_option.ViewModel );
        }
    }
}
