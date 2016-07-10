using MyLibrary;
using UnityEngine.UI;

namespace IdleFantasy {
    public class TaskUnitSelectView : GroupView {
        #region Inspector
        public Toggle Toggle;
        #endregion

        public delegate void UnitSelectedHandler( TaskUnitSelection unitSelection );
        public event UnitSelectedHandler UnitSelectedEvent;

        private TaskUnitSelection mTaskUnitSelection;

        public int NumUnitsRequired { get { return mTaskUnitSelection.NumUnitsRequired; } }

        public IUnit Unit { get { return mTaskUnitSelection.Unit; } }        

        public void Init( TaskUnitSelection i_selection ) {
            mTaskUnitSelection = i_selection;
            SetModel( i_selection.ViewModel );

            SetToggleGroup();
        }

        private void SetToggleGroup() {
            Toggle.group = GetComponentInParent<ToggleGroup>();
        }

        public void OnUnitSelected( bool i_selected ) {
            if ( UnitSelectedEvent != null ) {
                mTaskUnitSelection.SelectUnit( i_selected );
                UnitSelectedEvent( mTaskUnitSelection );
            }
        }
    }
}