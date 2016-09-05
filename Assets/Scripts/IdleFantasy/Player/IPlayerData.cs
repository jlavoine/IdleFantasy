﻿using MyLibrary;
using System.Collections.Generic;

namespace IdleFantasy {
    public interface IPlayerData {
        void Dispose();

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
        Dictionary<string, WorldMissionProgress> MissionProgress { get; }
        GameMetrics GameMetrics { get; }

        UnitUnlockPlanData UnitUnlockPlan { get; }
    }
}
