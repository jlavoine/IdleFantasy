using MyLibrary;

namespace IdleFantasy {
    public class UnitStatView : GroupView {

        public void Init( UnitStatPM i_statPM ) {
            SetModel( i_statPM.ViewModel );
        }
    }
}
