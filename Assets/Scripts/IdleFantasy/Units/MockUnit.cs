using System;

namespace IdleFantasy {
    public class MockUnit : IUnit {
        private int mUnitsPerTick;

        public MockUnit( int i_unitsPerTick ) {
            mUnitsPerTick = i_unitsPerTick;
        }

        public string GetID() {
            return "MOCK_UNIT";
        }

        public float GetProgressFromTimeElapsed( TimeSpan i_timeSpan ) {
            return mUnitsPerTick;
        }
    }
}