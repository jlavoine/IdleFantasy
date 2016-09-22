using MyLibrary;

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
            MyMessenger.AddListener<MissionData>( MissionKeys.MISSION_COMPLETED, OnMissionCompleted );
        }

        public void Dispose() {
            UnsubscribeFromMessages();
        }

        private void UnsubscribeFromMessages() {
            MyMessenger.RemoveListener<MissionData>( MissionKeys.MISSION_COMPLETED, OnMissionCompleted );
        }

        private void OnMissionCompleted( MissionData i_missionData ) {
            if ( i_missionData.Index == mData.Index ) {
                SetCompletedState( true );
                SetAreaAccessibility( false );
            }
        }

        private void SetUpModel() {
            SetTerrain();
            SetCompletedState( Progress.Completed );
            SetAreaAccessibility( !Progress.Completed );
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
    }
}