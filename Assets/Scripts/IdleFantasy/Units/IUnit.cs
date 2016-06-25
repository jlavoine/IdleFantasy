using System;

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

        float GetProgressFromTimeElapsed( TimeSpan i_timeSpan );

        bool HasStat( string i_stat );
        int GetBaseStat( string i_stat );       
    }
}