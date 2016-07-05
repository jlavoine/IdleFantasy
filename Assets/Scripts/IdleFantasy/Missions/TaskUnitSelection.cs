using MyLibrary;

namespace IdleFantasy {
    public class TaskUnitSelection  {
        private ViewModel mModel;
        public ViewModel ViewModel { get { return mModel; } }

        public TaskUnitSelection( IUnit i_unit, string i_stat, int i_powerRequirement ) {
            mModel = new ViewModel();

            SetUpModel( i_unit, i_stat, i_powerRequirement );
        }

        private void SetUpModel( IUnit i_unit, string i_stat, int i_powerRequirement ) {
            SetUnitRequiredProperty( i_unit );

            SetNumUnitsRequiredProperty( i_unit, i_stat, i_powerRequirement );            
        }

        private void SetUnitRequiredProperty( IUnit i_unit ) {
            mModel.SetProperty( MissionKeys.UNIT_FOR_TASK, i_unit.GetID() );
        }

        private void SetNumUnitsRequiredProperty( IUnit i_unit, string i_stat, int i_powerRequirement ) {
            int unitsRequired = StatCalculator.Instance.GetNumUnitsForRequirement( i_unit, i_stat, i_powerRequirement );
            mModel.SetProperty( MissionKeys.NUM_UNITS_FOR_TASK, unitsRequired );
        }
    }
}
