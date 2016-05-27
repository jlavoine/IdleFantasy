using System;
using MyLibrary;
using IdleFantasy.Data;

namespace IdleFantasy {
    public class Unit : IUnit {
        private UnitData mData;

        private ViewModel mModel;
        public ViewModel GetModel() {
            return mModel;
        }

        Upgradeable mLevel;
        public IUpgradeable Level {
            get { return mLevel; }
        }

        public int TrainingLevel {
            get { return mModel.GetPropertyValue<int>( "TrainingLevel" ); }

            set {
                if (value < 0) {
                    value = 0;
                }

                mModel.SetProperty( "TrainingLevel", value );
            }
        }

        public Unit( UnitData i_data, ViewModel i_model ) {
            mModel = i_model;
            mData = i_data;

            SetUnitLevel();

            SetUnitTraining();            
        }

        private void SetUnitTraining() {
            ITrainerManager trainerManager = PlayerManager.Data.TrainerManager;
            TrainingLevel = trainerManager.GetAssignedTrainers( mData.ID );
        }

        private void SetUnitLevel() {
            UnitProgress progress = PlayerManager.Data.UnitProgress[mData.ID];

            mLevel = new Upgradeable();
            mLevel.SetPropertyToUpgrade( mModel, mData.UnitLevel );
            Level.Value = progress.Level;
        }

        public string GetID() {
            return mData.ID;
        }

        public float GetProgressFromTimeElapsed( TimeSpan i_timeSpan ) {
            float progressPerSecond = mData.BaseProgressPerSecond / Level.Value;
            float progress = (float)(i_timeSpan.TotalSeconds * progressPerSecond);
            return progress;
        }

        public bool HasStat( string i_stat ) {
            return mData.Stats.ContainsKey( i_stat );
        }

        public int GetRoundedStat( string i_stat ) {
            float totalValue = 0f;
            StatInfo stat;
            if ( mData.Stats.TryGetValue( i_stat, out stat ) ) {
                float baseValue = stat.Base;
                totalValue = baseValue * Level.Value;
            }
            
            return (int)Math.Ceiling( totalValue );
        }
    }

}