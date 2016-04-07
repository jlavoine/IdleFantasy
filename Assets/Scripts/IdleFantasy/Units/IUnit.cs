using System;

namespace IdleFantasy {
    public interface IUnit {
        string GetID();

        float GetProgressFromTimeElapsed( TimeSpan i_timeSpan );
    }
}