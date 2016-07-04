using MyLibrary;

namespace IdleFantasy {
    public class MissionTask {
        private ViewModel mModel;
        public ViewModel ViewModel { get { return mModel; } }

        private MissionTaskData mData;
        public MissionTaskData Data {
            get { return mData; }
        }

        public MissionTask( MissionTaskData i_data ) {
            mModel = new ViewModel();
            mData = i_data;

            SetUpModel();
        }

        private void SetUpModel() {
            mModel.SetProperty( MissionKeys.DESCRIPTION, Data.DescriptionKey );
            mModel.SetProperty( MissionKeys.TASK_STAT, Data.StatRequirement );
            mModel.SetProperty( MissionKeys.TASK_POWER, Data.PowerRequirement );
        }
    }
}
