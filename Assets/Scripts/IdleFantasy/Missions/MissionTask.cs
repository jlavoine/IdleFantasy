﻿using MyLibrary;
using System.Collections.Generic;

namespace IdleFantasy {
    public class MissionTask {
        private ViewModel mModel;
        public ViewModel ViewModel { get { return mModel; } }

        private MissionTaskData mData;
        public MissionTaskData Data {
            get { return mData; }
        }

        private List<TaskUnitSelection> mUnitsEligibleForTask = new List<TaskUnitSelection>();
        public List<TaskUnitSelection> UnitsEligibleForTask { get { return mUnitsEligibleForTask; } }

        public MissionTask( MissionTaskData i_data, Dictionary<IUnit,int> i_promisedUnitsForMission ) {
            mModel = new ViewModel();
            mData = i_data;

            SetUpModel();
            AddUnitsEligibleForTask( i_promisedUnitsForMission );
        }

        private void SetUpModel() {
            mModel.SetProperty( MissionKeys.DESCRIPTION, Data.DescriptionKey );
            mModel.SetProperty( MissionKeys.TASK_STAT, Data.StatRequirement );
            mModel.SetProperty( MissionKeys.TASK_POWER, Data.PowerRequirement );
        }

        private void AddUnitsEligibleForTask( Dictionary<IUnit,int> i_promisedUnitsForMission ) {
            List<IUnit> unitsEligible = StatCalculator.Instance.GetUnitsWithStat( Data.StatRequirement );
            foreach ( IUnit unit in unitsEligible ) {
                TaskUnitSelection selection = new TaskUnitSelection( unit, Data.StatRequirement, Data.PowerRequirement, i_promisedUnitsForMission );
                mUnitsEligibleForTask.Add( selection );
            }
        }
    }
}