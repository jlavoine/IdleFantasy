using MyLibrary;
using UnityEngine;

namespace IdleFantasy {
    public class MissionTaskView : GroupView {
        public GameObject EligibleUnitPrefab;
        public GameObject EligibleUnitContent;

        public void Init( MissionTask i_task ) {
            SetModel( i_task.ViewModel );

            CreateEligibleUnitsList( i_task );
        }

        private void CreateEligibleUnitsList( MissionTask i_task  ) {
            foreach ( TaskUnitSelection unitSelection in i_task.UnitsEligibleForTask ) {
                GameObject unitObject = gameObject.InstantiateUI( EligibleUnitPrefab, EligibleUnitContent );
                TaskUnitSelectView view = unitObject.GetComponent<TaskUnitSelectView>();
                view.Init( unitSelection );
            }
        }
    }
}
