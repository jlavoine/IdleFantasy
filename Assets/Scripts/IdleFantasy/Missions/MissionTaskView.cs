using MyLibrary;
using UnityEngine;
using UnityEngine.UI;

namespace IdleFantasy {
    public class MissionTaskView : GroupView {
        #region Inspector
        public ToggleGroup ToggleGroup;
        #endregion

        public GameObject EligibleUnitPrefab;
        public GameObject EligibleUnitContent;

        private int mTaskIndex;

        private MissionTask mMissionTask;

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
                TaskUnitSelectView unitSelectView = unitObject.GetComponent<TaskUnitSelectView>();
                unitSelectView.Init( unitSelection );
                unitSelectView.UnitSelectedEvent += OnUnitSelectedForThisTask;
            }
        }

        private void ToggleUnitSelectionFromIndex( int i_missionTaskIndex ) {
            if ( PriorTaskWasSelected( i_missionTaskIndex ) ) {
                ToggleUnitSelectionOn();
            } else if ( NonPriorPreviousTaskWasSelected( i_missionTaskIndex ) ) {
                ToggleUnitSelectionOff();
            }
        }

        private bool PriorTaskWasSelected( int i_missionTaskIndex ) {
            return i_missionTaskIndex == mTaskIndex - 1;
        }

        private bool NonPriorPreviousTaskWasSelected( int i_missionTaskIndex ) {
            return i_missionTaskIndex < mTaskIndex;
        }

        private void ToggleUnitSelectionOff() {
            EligibleUnitContent.SetActive( false );
            UnselectAnyToggledSelections();
        }

        private void ToggleUnitSelectionOn() {
            EligibleUnitContent.SetActive( true );
            UnselectAnyToggledSelections();

            foreach ( TaskUnitSelection unitSelection in mMissionTask.UnitsEligibleForTask ) {
                unitSelection.RecalculateProperties();
            }
        }

        private void UnselectAnyToggledSelections() {
            ToggleGroup.SetAllTogglesOff();
        }

        private void OnUnitSelectedForThisTask( TaskUnitSelection i_selection ) {
            UnityEngine.Debug.LogError( "Hey a selection" );
            MyMessenger.Send( MissionKeys.UNIT_SELECTED_EVENT, mTaskIndex );
        }

        private void OnUnitSelected( int i_missionTaskIndexForSelection ) {
            ToggleUnitSelectionFromIndex( i_missionTaskIndexForSelection );
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            UnsubscribeFromMessages();
        }

        private void SubscribeToMessages() {
            MyMessenger.AddListener<int>( MissionKeys.UNIT_SELECTED_EVENT, OnUnitSelected );
        }

        private void UnsubscribeFromMessages() {
            MyMessenger.RemoveListener<int>( MissionKeys.UNIT_SELECTED_EVENT, OnUnitSelected );
        }
    }
}
