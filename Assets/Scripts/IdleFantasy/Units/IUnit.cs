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
    }
}