using System.Collections.Generic;

namespace MyLibrary {
    public class TutorialStep : GroupView {
        public const string TEXT_PROPERTY = "Text";

        public string StepName;
        public string StepTextKey;
        public List<string> SendMessagesOnStart;
        public string EndStepOnMessage;

        void Start() {
            InitAndSetModel();
            SendStartingMessages();
            SubscribeToMessages();
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            UnsubscribeFromMessages();
        }

        private void SubscribeToMessages() {
            MyMessenger.AddListener( EndStepOnMessage, EndStep );
        }

        private void UnsubscribeFromMessages() {
            MyMessenger.RemoveListener( EndStepOnMessage, EndStep );
        }

        private void EndStep() {
            MyMessenger.Send( Tutorial.END_STEP_MESSAGE );
        }

        private void SendStartingMessages() {
            foreach ( string message in SendMessagesOnStart ) {
                MyMessenger.Send( message );
            }
        }

        private void InitAndSetModel() {
            ViewModel model = new ViewModel();
            model.SetProperty( TEXT_PROPERTY, StringTableManager.Get( StepTextKey ) );

            SetModel( model );
        }
    }
}