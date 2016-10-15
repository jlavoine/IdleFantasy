using MyLibrary;
using System.Collections.Generic;

namespace IdleFantasy {
    public class MapArea {
        private ViewModel mModel = new ViewModel();
        public ViewModel ViewModel { get { return mModel; } }

        private MapAreaData mData;
        public MapAreaData Data { get { return mData; } }

        private SingleMissionProgress mProgress;
        public SingleMissionProgress Progress { get { return mProgress; } }

        public MapArea( MapAreaData i_areaData, SingleMissionProgress i_missionProgressForArea ) {            
            mData = i_areaData;
            mProgress = i_missionProgressForArea;

            SubscribeToMessages();

            SetUpModel();
        }

        private void SubscribeToMessages() {
            MyMessenger.AddListener<MissionData>( MissionKeys.MISSION_CLIENT_COMPLETE_PROCESSED, OnMissionCompleted );
        }

        public void Dispose() {
            UnsubscribeFromMessages();
        }

        private void UnsubscribeFromMessages() {
            MyMessenger.RemoveListener<MissionData>( MissionKeys.MISSION_CLIENT_COMPLETE_PROCESSED, OnMissionCompleted );
        }

        private void OnMissionCompleted( MissionData i_missionData ) {
            if ( i_missionData.Index == mData.Index ) {
                SetCompletedState( true );
                SetAreaAccessibility( false );                
            }

            SetAreaUnlockedState(); // "inefficient" to check every area
            SetAreaAccessibility( !Progress.Completed && IsAreaUnlocked() );
        }

        private void SetUpModel() {
            SetTerrain();
            SetCompletedState( Progress.Completed );
            SetAreaAccessibility( !Progress.Completed && IsAreaUnlocked() );
            SetAreaUnlockedState();
        }

        private void SetTerrain() {
            ViewModel.SetProperty( MapViewProperties.TERRAIN_TYPE, Data.Terrain.ToString() );
        }

        private void SetCompletedState( bool i_state ) {
            ViewModel.SetProperty( MapViewProperties.AREA_COMPLETED, i_state );
        }

        private void SetAreaAccessibility( bool i_isAccessible ) {
            ViewModel.SetProperty( MapViewProperties.AREA_ACCESS, i_isAccessible );
        }

        private void SetAreaUnlockedState() {
            ViewModel.SetProperty( MapViewProperties.AREA_UNLOCKED, IsAreaUnlocked() );
        }

        private bool IsAreaUnlocked() {
            IWorldMissionProgress missionProgressForWorld = PlayerManager.Data.GetMissionProgressForWorld( BackendConstants.WORLD_BASE );

            if ( Data.Index == 12 || Progress.IsComplete() ) {
                return true;
            } else if ( ( Data.Index + 1 ) % 5 == 0 ) {
                return missionProgressForWorld.IsMissionWithIndexComplete( Data.Index - 1 )
                    || missionProgressForWorld.IsMissionWithIndexComplete( Data.Index + 5 )
                    || missionProgressForWorld.IsMissionWithIndexComplete( Data.Index - 5 );
            } else if ( ( Data.Index + 2 ) % 5 == 0 || ( Data.Index + 3 ) % 5 == 0 || ( Data.Index + 4 ) % 5 == 0 ) {
                return missionProgressForWorld.IsMissionWithIndexComplete( Data.Index - 1 )
                    || missionProgressForWorld.IsMissionWithIndexComplete( Data.Index + 1 )
                    || missionProgressForWorld.IsMissionWithIndexComplete( Data.Index + 5 )
                    || missionProgressForWorld.IsMissionWithIndexComplete( Data.Index - 5 );
            } else if ( Data.Index % 5 == 0 ) {
                return missionProgressForWorld.IsMissionWithIndexComplete( Data.Index + 1 )
                    || missionProgressForWorld.IsMissionWithIndexComplete( Data.Index + 5 )
                    || missionProgressForWorld.IsMissionWithIndexComplete( Data.Index - 5 );
            } else {
                return false;
            }
        }
    }
}