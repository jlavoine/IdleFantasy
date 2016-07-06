using MyLibrary;
using UnityEngine;

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
            SetNumUnitsRequiredColorProperty( i_unit );
            SetInteractableProperty( i_unit ); 
        }

        private void SetUnitRequiredProperty( IUnit i_unit ) {
            mModel.SetProperty( MissionKeys.UNIT_FOR_TASK, i_unit.GetID() );
        }

        private void SetNumUnitsRequiredProperty( IUnit i_unit, string i_stat, int i_powerRequirement ) {
            int unitsRequired = StatCalculator.Instance.GetNumUnitsForRequirement( i_unit, i_stat, i_powerRequirement );
            mModel.SetProperty( MissionKeys.NUM_UNITS_FOR_TASK, unitsRequired );
        }

        private void SetNumUnitsRequiredColorProperty( IUnit i_unit ) {
            bool hasEnoughUnits = HasEnoughUnits( i_unit );
            string colorConstantKey = hasEnoughUnits ? ConstantKeys.ENOUGH_UNITS_COLOR : ConstantKeys.NOT_ENOUGH_UNITS_COLOR;
            Color unitTextColor = Constants.GetConstant<Color>( colorConstantKey );

            mModel.SetProperty( MissionKeys.NUM_UNITS_FOR_TASK_COLOR, unitTextColor );
        }

        private void SetInteractableProperty( IUnit i_unit ) {
            bool hasEnoughUnits = HasEnoughUnits( i_unit );

            mModel.SetProperty( MissionKeys.IS_UNIT_SELECTABLE, hasEnoughUnits );
        }

        private bool HasEnoughUnits( IUnit i_unit ) {
            int unitsRequired = mModel.GetPropertyValue<int>( MissionKeys.NUM_UNITS_FOR_TASK );
            int numUnitsOwned = BuildingUtils.GetNumUnits( i_unit );
            bool hasEnoughUnits = numUnitsOwned >= unitsRequired;

            return hasEnoughUnits;
        }
    }
}
