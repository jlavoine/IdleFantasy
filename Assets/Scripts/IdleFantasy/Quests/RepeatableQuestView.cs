using MyLibrary;

namespace IdleFantasy {
    public class RepeatableQuestView : GroupView {
        public MissionView MissionUI;
        
        private RepeatableQuestModel mQuestModel;

        public void Init( RepeatableQuestModel i_model ) {
            mQuestModel = i_model;
            SetModel( i_model.ViewModel );
            MissionUI.Init( mQuestModel.Mission );       
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            mQuestModel.Dispose();
        }
    }
}