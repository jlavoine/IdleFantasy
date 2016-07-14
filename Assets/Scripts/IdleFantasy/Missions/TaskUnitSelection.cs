using MyLibrary;
using UnityEngine;
using System.Collections.Generic;

namespace IdleFantasy {
    public class TaskUnitSelection {
        #region Variables
        private ViewModel mModel;
        public ViewModel ViewModel { get { return mModel; } }

        private IUnit mUnit;
        public IUnit Unit { get { return mUnit; } }

        private string mStat;
        public string Stat { get { return mStat; } }

        private int mPowerRequirement;
        public int PowerRequirement { get { return mPowerRequirement; } }

        private Dictionary<IUnit, int> mPromisedUnits = new Dictionary<IUnit, int>();
        #endregion

        #region Public Properties
        public int NumUnitsRequired { get { return mModel.GetPropertyValue<int>( MissionKeys.NUM_UNITS_FOR_TASK ); } }
        #endregion

        public TaskUnitSelection( IUnit i_unit, string i_stat, int i_powerRequirement, Dictionary<IUnit,int> i_promisedUnits ) {
            mPromisedUnits = i_promisedUnits;
            mUnit = i_unit;
            mStat = i_stat;
            mPowerRequirement = i_powerRequirement;
            mModel = new ViewModel();

            SetUpModel();
        }

        #region Setting Properties
        public void RecalculateProperties() {
            SetUpModel();
        }

        private void SetUpModel() {
            SetUnitRequiredProperty();
            SetNumUnitsRequiredProperty();
            SetNumUnitsRequiredColorProperty();
            SetInteractableProperty(); 
        }

        private void SetUnitRequiredProperty() {
            mModel.SetProperty( MissionKeys.UNIT_FOR_TASK, Unit.GetID() );
        }

        private void SetNumUnitsRequiredProperty() {
            int unitsRequired = StatCalculator.Instance.GetNumUnitsForRequirement( Unit, Stat, PowerRequirement );
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
        #endregion

        public bool HasEnoughUnits() {
            int numUnitsOwned = BuildingUtilsManager.Utils.GetNumUnits( Unit );
            int numUnitsPromised = mPromisedUnits.ContainsKey( Unit ) ? mPromisedUnits[Unit] : 0;
            int numUnitsAvailable = numUnitsOwned - numUnitsPromised;
            bool hasEnoughUnits = numUnitsAvailable >= NumUnitsRequired;            

            return hasEnoughUnits;
        }

        public void UnitSelected( bool i_selected ) {
            if ( !HasEnoughUnits() ) {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "Unit selected, but not enough units: " + mUnit.GetID(), "TaskUnitSelection" );
                return;
            }

            if ( !i_selected && mPromisedUnits.ContainsKey( Unit ) && mPromisedUnits[Unit] == 0 ) {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "Unit unselected, but it was never selected properly: " + mUnit.GetID(), "TaskUnitSelection" );
                return;
            }

            int promisedUnits = 0;
            mPromisedUnits.TryGetValue( Unit, out promisedUnits );

            if ( i_selected ) {
                promisedUnits += NumUnitsRequired;
            } else {
                promisedUnits -= NumUnitsRequired;
            }

            mPromisedUnits[Unit] = promisedUnits;
        }
    }
}
