using MyLibrary;

namespace IdleFantasy {
    public class Mission {
        private ViewModel mModel;
        public ViewModel ViewModel { get { return mModel; } }

        private MissionData mData;
        public MissionData Data {
            get { return mData; }
        }

        public Mission( MissionData i_data ) {         
            mModel = new ViewModel();
            mData = i_data;

            SetUpModel();            
        }

        private void SetUpModel() {
            mModel.SetProperty( MissionKeys.DESCRIPTION, Data.DescriptionKey );
        }
    }
}
