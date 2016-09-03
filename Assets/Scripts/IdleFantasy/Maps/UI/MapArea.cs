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

            SetUpModel();
        }

        private void SetUpModel() {
            SetTerrain();
            SetCompletedState();
        }

        private void SetTerrain() {
            ViewModel.SetProperty( MapViewProperties.TERRAIN_TYPE, Data.Terrain.ToString() );
        }

        private void SetCompletedState() {
            ViewModel.SetProperty( MapViewProperties.AREA_COMPLETED, Progress.Completed );
        }
    }
}