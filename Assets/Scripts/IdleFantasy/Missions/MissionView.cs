using MyLibrary;
using UnityEngine;

namespace IdleFantasy {
    public class MissionView : GroupView {
        public GameObject MissionTaskPrefab;
        public GameObject MissionTaskContent;

        public void Init( Mission i_mission ) {
            SetModel( i_mission.ViewModel );

            CreateAndInitMissionTasks( i_mission  );
        }

        private void CreateAndInitMissionTasks( Mission i_mission ) {
            foreach ( MissionTask task in i_mission.Tasks ) {
                GameObject taskObject = gameObject.InstantiateUI( MissionTaskPrefab, MissionTaskContent );
                MissionTaskView taskView = taskObject.GetComponent<MissionTaskView>();
                taskView.Init( task );
            }
        }
    }
}
