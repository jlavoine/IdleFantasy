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

        float GetProgressFromTimeElapsed( TimeSpan i_timeSpan );

        bool HasStat( string i_stat );
        int GetRoundedStat( string i_stat );
    }
}