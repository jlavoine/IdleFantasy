using System;

namespace IdleFantasy {
    public class MockUnit : IUnit {
        private float mUnitsPerSecond;
        private int mTrainingLevel = 1;

        public MockUnit( float i_unitsPerSecond ) {
            mUnitsPerSecond = i_unitsPerSecond;
        }

        public IUpgradeable Level {
            get {
                return new MockUpgrade();
            }
        }

        public int TrainingLevel {
            get {
                return mTrainingLevel;
            }

            set {
                mTrainingLevel = value;
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

        public bool HasStat( string i_stat ) {
            return true;
        }

        public int GetRoundedStat( string i_stat ) {
            return TrainingLevel;
        }
    }
}