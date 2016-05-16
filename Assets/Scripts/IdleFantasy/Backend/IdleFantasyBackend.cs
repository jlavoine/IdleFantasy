using MyLibrary;

namespace IdleFantasy {
    public class IdleFantasyBackend : PlayFabBackend, IBackend {

        public IdleFantasyBackend( IMessageService i_messenger ) : base(i_messenger) {
            mMessenger = i_messenger;
        }

        public void MakeUpgradeCall() {

        }
    }
}