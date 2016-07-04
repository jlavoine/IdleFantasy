using MyLibrary;
using System.Collections.Generic;

namespace IdleFantasy {
    public class Mission {
        private ViewModel mModel;
        public ViewModel ViewModel { get { return mModel; } }

        private MissionData mData;
        public MissionData Data { get { return mData; } }

        private List<MissionTask> mTasks;
        public List<MissionTask> Tasks { get { return mTasks; } }

        public Mission( MissionData i_data ) {         
            mModel = new ViewModel();
            mData = i_data;

            SetUpModel();

            CreateMissionTasks();         
        }

        private void SetUpModel() {
            mModel.SetProperty( MissionKeys.DESCRIPTION, Data.DescriptionKey );
        }

        private void CreateMissionTasks() {
            mTasks = new List<MissionTask>();
            foreach ( MissionTaskData taskData in Data.Tasks ) {
                AddMissionTask( taskData );
            }
        }

        private void AddMissionTask( MissionTaskData i_data ) {
            mTasks.Add( new MissionTask( i_data ) );
        }
    }
}
