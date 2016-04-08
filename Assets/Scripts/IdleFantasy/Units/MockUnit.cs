using System;

namespace IdleFantasy {
    public class MockUnit : IUnit {
        private float mUnitsPerSecond;

        public MockUnit( float i_unitsPerSecond ) {
            mUnitsPerSecond = i_unitsPerSecond;
        }

        public IUpgradeable Level {
            get {
                return new MockUpgrade();
            }
        }

        public string GetID() {
            return "MOCK_UNIT";
        }

        public float GetProgressFromTimeElapsed( TimeSpan i_timeSpan ) {
            float progress = (float) ( mUnitsPerSecond * i_timeSpan.TotalSeconds );
            UnityEngine.Debug.Log( progress );
            return progress;
        }
    }
}