using MyLibrary;
using System.Collections.Generic;

namespace IdleFantasy {
    public class Mission {
        private ViewModel mModel;
        public ViewModel ViewModel { get { return mModel; } }

        private MissionData mData;
        public MissionData Data { get { return mData; } }

        private List<MissionTask> mTasks = new List<MissionTask>();
        public List<MissionTask> Tasks { get { return mTasks; } }

        private MissionProposal mMissionProposal = new MissionProposal();
        public MissionProposal MissionProposal { get { return mMissionProposal; } set { mMissionProposal = value; } }

        public Mission( MissionData i_data ) {         
            mModel = new ViewModel();
            mData = i_data;
            
            SetUpModel();

            CreateMissionTasks();         
        }

        private void SetUpModel() {
            mModel.SetProperty( MissionKeys.GOLD_REWARD, mData.GoldReward );
        }

        private void CreateMissionTasks() {
            foreach ( MissionTaskData taskData in Data.Tasks ) {
                AddMissionTask( taskData );
            }
        }

        private void AddMissionTask( MissionTaskData i_data ) {
            mTasks.Add( new MissionTask( i_data, MissionProposal ) );
        }

        public void CompleteMission() {
            AlterLocalUnits();            
            SendCompletionToServer();

            SendMissionCompletionMessage();
        }

        private void AlterLocalUnits() {
            foreach ( KeyValuePair<string, int> promisedUnitPair in MissionProposal.PromisedUnits ) {
                BuildingUtilsManager.Utils.AlterUnitCount( promisedUnitPair.Key, -promisedUnitPair.Value );
            }
        }

        private void SendMissionCompletionMessage() {
            MyMessenger.Send( MissionKeys.MISSION_COMPLETED, mData );
        }

        private void SendCompletionToServer() {
            BackendManager.Backend.CompleteMission( mData.MissionCategory, mData.Index, MissionProposal.TaskProposals );
        }    
    }
}
