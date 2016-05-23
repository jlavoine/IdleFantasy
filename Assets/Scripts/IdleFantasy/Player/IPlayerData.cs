using MyLibrary;

namespace IdleFantasy {
    public interface IPlayerData {
        ViewModel GetViewModel();

        int Gold {
            get;
            set;
        }

        //BuildingProgress GetBuildingProgress( string i_ID );

        object GetData( string i_key );

        /*ITrainerData TrainerData {
            get;
        }*/
    }
}
