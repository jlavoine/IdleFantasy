using MyLibrary;

namespace IdleFantasy {
    public interface IPlayerData {
        ViewModel GetViewModel();

        int Gold {
            get;
            set;
        }

        object GetData( string i_key );

        /*ITrainerData TrainerData {
            get;
        }*/
    }
}
