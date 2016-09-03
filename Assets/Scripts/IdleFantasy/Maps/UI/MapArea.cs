using MyLibrary;

namespace IdleFantasy {
    public class MapArea {
        private ViewModel mModel = new ViewModel();
        public ViewModel ViewModel { get { return mModel; } }

        private MapAreaData mData;
        public MapAreaData Data {
            get { return mData; }
        }

        public MapArea( MapAreaData i_areaData ) {            
            mData = i_areaData;

            SetUpModel();
        }

        private void SetUpModel() {
            SetTerrain();
        }

        private void SetTerrain() {
            ViewModel.SetProperty( MapViewProperties.TERRAIN_TYPE, Data.Terrain.ToString() );
        }
    }
}