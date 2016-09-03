﻿using MyLibrary;
using System.Collections.Generic;

namespace IdleFantasy {
    public interface IPlayerData {
        ViewModel GetViewModel();

        int Gold {
            get;
            set;
        }
        //object GetData( string i_key );

        List<Guild> Guilds { get; }
        List<Building> Buildings { get; }
        ITrainerManager TrainerManager { get; }
        Dictionary<string, MapData> Maps { get; }
    }
}
