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

        private int mTaskIndex;
        public int TaskIndex { get { return mTaskIndex; } }

        private string mStat;
        public string Stat { get { return mStat; } }

        private int mPowerRequirement;
        public int PowerRequirement { get { return mPowerRequirement; } }

        private MissionTaskProposal mTaskProposal;
        public MissionTaskProposal TaskProposal { get { return mTaskProposal; } }

        private MissionProposal mMissionProposal = new MissionProposal();
        #endregion

        #region Public Properties
        public int NumUnitsRequired { get { return mModel.GetPropertyValue<int>( MissionKeys.NUM_UNITS_FOR_TASK ); } }
        #endregion

        public TaskUnitSelection( IUnit i_unit, MissionTaskData i_taskData, MissionProposal i_proposal ) {
            mMissionProposal = i_proposal;
            mUnit = i_unit;
            mTaskIndex = i_taskData.Index;
            mStat = i_taskData.StatRequirement;
            mPowerRequirement = i_taskData.PowerRequirement;            
            mModel = new ViewModel();

            SetUpModel();

            mTaskProposal = new MissionTaskProposal( mTaskIndex, mUnit.GetID(), NumUnitsRequired );
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
            int numUnitsPromised = mMissionProposal.PromisedUnits.ContainsKey( Unit.GetID() ) ? mMissionProposal.PromisedUnits[Unit.GetID()] : 0;
            int numUnitsAvailable = numUnitsOwned - numUnitsPromised;
            bool hasEnoughUnits = numUnitsAvailable >= NumUnitsRequired;            

            return hasEnoughUnits;
        }

        public void UnitSelected( bool i_selected ) {
            if ( !HasEnoughUnits() ) {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "Unit selected, but not enough units: " + mUnit.GetID(), "TaskUnitSelection" );
                return;
            }

            if ( !i_selected && mMissionProposal.PromisedUnits.ContainsKey( Unit.GetID() ) && mMissionProposal.PromisedUnits[Unit.GetID()] == 0 ) {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "Unit unselected, but it was never selected properly: " + mUnit.GetID(), "TaskUnitSelection" );
                return;
            }

            if ( i_selected ) {
                mMissionProposal.AddProposal( TaskIndex, mTaskProposal );
            } else {
                mMissionProposal.RemoveProposal( TaskIndex, mTaskProposal );
            }
        }
    }
}
