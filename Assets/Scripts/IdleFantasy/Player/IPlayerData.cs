using MyLibrary;
using System.Collections.Generic;

namespace IdleFantasy {
    public interface IPlayerData {
        ViewModel GetViewModel();

        int Gold {
            get;
            set;
        }
        object GetData( string i_key );

        List<Guild> Guilds { get; }
        ITrainerManager TrainerManager { get; }
        Dictionary<string, UnitProgress> UnitProgress { get; }
        Dictionary<string, BuildingProgress> BuildingProgress { get; }
    }
}
