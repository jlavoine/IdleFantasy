using System;

namespace IdleFantasy {
    public interface IUnit {
        string GetID();

        IUpgradeable Level {
            get;
        }

        float GetProgressFromTimeElapsed( TimeSpan i_timeSpan );
    }
}