using MyLibrary;

namespace IdleFantasy {
    public interface IPlayerData {
        ViewModel GetViewModel();

        int Gold {
            get;
            set;
        }
    }
}
