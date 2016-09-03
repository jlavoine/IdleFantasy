using MyLibrary;

namespace IdleFantasy {
    public class Map {
        private ViewModel mModel;
        public ViewModel ViewModel { get { return mModel; } }

        private MapData mData;
        public MapData Data { get { return mData; } }

        public Map( MapData i_data ) {
            mModel = new ViewModel();
            mData = i_data;

            SetUpModel();
        }

        private void SetUpModel() {
            SetMapName();            
        }

        private void SetMapName() {
            mModel.SetProperty( MapViewProperties.NAME, Data.Name.GetStringName() );
        }
    }
}
