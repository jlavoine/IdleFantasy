using MyLibrary;

namespace IdleFantasy {
    public class TaskUnitSelectView : GroupView {
        public delegate void UnitSelectedHandler( TaskUnitSelection unitSelection );
        public event UnitSelectedHandler UnitSelectedEvent;

        private TaskUnitSelection mTaskUnitSelection;

        public void Init( TaskUnitSelection i_selection ) {
            mTaskUnitSelection = i_selection;
            SetModel( i_selection.ViewModel );
        }

        public void OnUnitSelected() {
            if ( UnitSelectedEvent != null ) {
                UnitSelectedEvent( mTaskUnitSelection );
            }
        }
    }
}