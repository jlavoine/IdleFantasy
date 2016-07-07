using MyLibrary;
using UnityEngine;

namespace IdleFantasy {
    public class TaskUnitSelection  {
        private ViewModel mModel;
        public ViewModel ViewModel { get { return mModel; } }

        private IUnit mUnit;
        public IUnit Unit { get { return mUnit; } }

        public TaskUnitSelection( IUnit i_unit, string i_stat, int i_powerRequirement ) {
            mUnit = i_unit;
            mModel = new ViewModel();

            SetUpModel( i_stat, i_powerRequirement );
        }

        private void SetUpModel( string i_stat, int i_powerRequirement ) {
            SetUnitRequiredProperty();
            SetNumUnitsRequiredProperty( i_stat, i_powerRequirement );
            SetNumUnitsRequiredColorProperty();
            SetInteractableProperty(); 
        }

        private void SetUnitRequiredProperty() {
            mModel.SetProperty( MissionKeys.UNIT_FOR_TASK, Unit.GetID() );
        }

        private void SetNumUnitsRequiredProperty(string i_stat, int i_powerRequirement ) {
            int unitsRequired = StatCalculator.Instance.GetNumUnitsForRequirement( Unit, i_stat, i_powerRequirement );
            mModel.SetProperty( MissionKeys.NUM_UNITS_FOR_TASK, unitsRequired );
        }

        private void SetNumUnitsRequiredColorProperty() {
            bool hasEnoughUnits = HasEnoughUnits();
            string colorConstantKey = hasEnoughUnits ? ConstantKeys.ENOUGH_UNITS_COLOR : ConstantKeys.NOT_ENOUGH_UNITS_COLOR;
            Color unitTextColor = Constants.GetConstant<Color>( colorConstantKey );

            mModel.SetProperty( MissionKeys.NUM_UNITS_FOR_TASK_COLOR, unitTextColor );
        }

        private void SetInteractableProperty() {
            bool hasEnoughUnits = HasEnoughUnits();

            mModel.SetProperty( MissionKeys.IS_UNIT_SELECTABLE, hasEnoughUnits );
        }

        private bool HasEnoughUnits() {
            int unitsRequired = mModel.GetPropertyValue<int>( MissionKeys.NUM_UNITS_FOR_TASK );
            int numUnitsOwned = BuildingUtils.GetNumUnits( Unit );
            bool hasEnoughUnits = numUnitsOwned >= unitsRequired;

            return hasEnoughUnits;
        }
    }
}
