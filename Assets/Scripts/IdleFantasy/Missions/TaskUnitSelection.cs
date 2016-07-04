using MyLibrary;

namespace IdleFantasy {
    public class TaskUnitSelection  {
        private ViewModel mModel;
        public ViewModel ViewModel { get { return mModel; } }

        public TaskUnitSelection( IUnit i_unit, string i_stat ) {
            mModel = new ViewModel();

            SetUpModel( i_unit, i_stat );
        }

        private void SetUpModel( IUnit i_unit, string i_stat ) {
            mModel.SetProperty( MissionKeys.UNIT_FOR_TASK, i_unit.GetID() );
        }
    }
}
