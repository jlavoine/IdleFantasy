using MyLibrary;

namespace IdleFantasy {
    public interface IBackend : IBasicBackend {
        void MakeUpgradeCall( string i_className, string i_targetID, string i_upgradeID );
    }
}