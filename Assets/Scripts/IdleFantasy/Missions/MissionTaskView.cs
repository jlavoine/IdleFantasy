using MyLibrary;
using UnityEngine;

namespace IdleFantasy {
    public class MissionTaskView : GroupView {
        public GameObject EligibleUnitPrefab;
        public GameObject EligibleUnitContent;

        private int mTaskIndex;

        private MissionTask mMissionTask;
        private TaskUnitSelectView mUnitSelectView;

        public void Init( MissionTask i_task, int i_index ) {
            mMissionTask = i_task;
            mTaskIndex = i_index;
            SetModel( i_task.ViewModel );

            SubscribeToMessages();

            CreateEligibleUnitsList();

            if ( i_index != 0 ) {
                ToggleUnitSelectionOff();
            }
        }        

        private void CreateEligibleUnitsList() {
            foreach ( TaskUnitSelection unitSelection in mMissionTask.UnitsEligibleForTask ) {
                GameObject unitObject = gameObject.InstantiateUI( EligibleUnitPrefab, EligibleUnitContent );
                mUnitSelectView = unitObject.GetComponent<TaskUnitSelectView>();
                mUnitSelectView.Init( unitSelection );
                mUnitSelectView.UnitSelectedEvent += OnUnitSelectedForThisTask;
            }
        }

        private void ToggleUnitSelectionFromIndex( int i_missionTaskIndex ) {
            if ( i_missionTaskIndex == mTaskIndex - 1 ) {
                ToggleUnitSelectionOn();
            } else if ( i_missionTaskIndex < mTaskIndex ) {
                ToggleUnitSelectionOff();
            }
        }

        private void ToggleUnitSelectionOff() {
            EligibleUnitContent.SetActive( false );
        }

        private void ToggleUnitSelectionOn() {
            EligibleUnitContent.SetActive( true );
        }

        private void OnUnitSelectedForThisTask( TaskUnitSelection i_selection ) {
            MyMessenger.Send( MissionKeys.UNIT_SELECTED_EVENT, mTaskIndex );
        }

        private void OnUnitSelected( int i_missionTaskIndexForSelection ) {
            ToggleUnitSelectionFromIndex( i_missionTaskIndexForSelection );
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            UnsubscribeFromEvents();
            UnsubscribeFromMessages();
        }

        private void SubscribeToMessages() {
            MyMessenger.AddListener<int>( MissionKeys.UNIT_SELECTED_EVENT, OnUnitSelected );
        }

        private void UnsubscribeFromMessages() {
            MyMessenger.RemoveListener<int>( MissionKeys.UNIT_SELECTED_EVENT, OnUnitSelected );
        }

        private void UnsubscribeFromEvents() {
            mUnitSelectView.UnitSelectedEvent -= OnUnitSelectedForThisTask;
        }
    }
}
