using System;
using System.Collections.Generic;

namespace IdleFantasy {
    public interface IUnit {
        string GetID();

        IUpgradeable Level {
            get;
        }

        int TrainingLevel {
            get;
            set;
        }
        bool CanTrain();

        string GetName();

        float GetProgressFromTimeElapsed( TimeSpan i_timeSpan );

        bool HasStat( string i_stat );
        int GetBaseStat( string i_stat );

        List<string> GetStats();
    }
}