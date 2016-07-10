using MyLibrary;
using UnityEngine;

namespace IdleFantasy {
    public class MissionView : GroupView {
        public GameObject MissionTaskPrefab;
        public GameObject MissionTaskContent;

        private ViewModel mViewModel;

        private Mission mMission;

        public void Init( Mission i_mission ) {
            mViewModel = i_mission.ViewModel;
            mMission = i_mission;
            UpdateMissionReadiness( -1 );

            SetModel( mViewModel );

            SubscribeToMessages();

            CreateAndInitMissionTasks( i_mission );
        }

        private void CreateAndInitMissionTasks( Mission i_mission ) {
            int missionTaskIndex = 0;
            foreach ( MissionTask task in i_mission.Tasks ) {
                GameObject taskObject = gameObject.InstantiateUI( MissionTaskPrefab, MissionTaskContent );
                MissionTaskView taskView = taskObject.GetComponent<MissionTaskView>();
                taskView.Init( task, missionTaskIndex );
                missionTaskIndex++;
            }
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            UnsubscribeFromMessages();
        }

        private void OnUnitSelected( int i_taskIndex ) {
            UpdateMissionReadiness( i_taskIndex );
        }

        private void UpdateMissionReadiness( int i_taskSelectedIndex ) {
            bool isReady = i_taskSelectedIndex + 1 == mMission.Tasks.Count;
            mViewModel.SetProperty( MissionKeys.MISSION_READY, isReady );
        }

        public void CompleteMission() {
            mMission.CompleteMission();
        }

        private void SubscribeToMessages() {
            MyMessenger.AddListener<int>( MissionKeys.UNIT_SELECTED_EVENT, OnUnitSelected );
        }

        private void UnsubscribeFromMessages() {
            MyMessenger.RemoveListener<int>( MissionKeys.UNIT_SELECTED_EVENT, OnUnitSelected );
        }
    }
}
