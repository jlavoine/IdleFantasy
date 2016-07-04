using MyLibrary;

namespace IdleFantasy {
    public class TaskUnitSelectView : GroupView {

        public void Init( TaskUnitSelection i_selection ) {
            SetModel( i_selection.ViewModel );
        }
    }
}