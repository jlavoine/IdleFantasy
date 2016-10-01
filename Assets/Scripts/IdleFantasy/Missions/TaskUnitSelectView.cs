using MyLibrary;
using UnityEngine.UI;

namespace IdleFantasy {
    public class TaskUnitSelectView : GroupView {
        public const string UNIT_SELECTED = "UnitSelected"; // for tutorials

        #region Inspector
        public Toggle Toggle;
        #endregion

        private bool mIsOn = false; // Need this because you can't tell if a toggle is on or not on selection...!

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
            MyMessenger.Send( UNIT_SELECTED );

            // this effectively blocks selection of a unit that is already selected. Why Unity, WHY???
            if ( mIsOn && i_selected ) {
                return;
            }
            
            mIsOn = i_selected;
            if ( UnitSelectedEvent != null ) {
                mTaskUnitSelection.UnitSelected( i_selected );
                UnitSelectedEvent( mTaskUnitSelection );
            }
        }
    }
}