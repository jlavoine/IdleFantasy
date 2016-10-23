using System;
using MyLibrary;
using IdleFantasy.Data;
using System.Collections.Generic;

namespace IdleFantasy {
    public class Unit : IUnit {
        private UnitData mData;
        private UnitProgress mProgress;

        private ViewModel mModel;
        public ViewModel GetModel() {
            return mModel;
        }

        Upgradeable mLevel;
        public IUpgradeable Level {
            get { return mLevel; }
        }

        public string GetName() {
            return mData.GetName();
        }

        public int TrainingLevel {
            get { return mModel.GetPropertyValue<int>( "TrainingLevel" ); }

            set {
                if (value < 0) {
                    value = 0;
                } else if ( value > GetMaxTrainingLevel() ) {
                    value = GetMaxTrainingLevel();
                }

                mModel.SetProperty( "TrainingLevel", value );
            }
        }

        public bool CanAddTrainer {
            get { return mModel.GetPropertyValue<bool>( "CanAddTrainer" ); }
            set { mModel.SetProperty( "CanAddTrainer", value ); }
        }

        public bool CanRemoveTrainer {
            get { return mModel.GetPropertyValue<bool>( "CanRemoveTrainer" ); }
            set { mModel.SetProperty( "CanRemoveTrainer", value ); }
        }

        public Unit( UnitData i_data, UnitProgress i_progress, ViewModel i_model ) {
            mModel = i_model;
            mData = i_data;
            mProgress = i_progress;

            SetUnitLevel();
            SetUnitTraining();

            Level.UpgradeCompleteEvent += OnUpgraded;

            SubscribeToMessages();
        }

        public void Dispose() {
            Level.UpgradeCompleteEvent -= OnUpgraded;
            UnsubscribeFromMessages();
        }

        private void SubscribeToMessages() {
            EasyMessenger.Instance.AddListener<ITrainerManager>( TrainerManager.AVAILABLE_TRAINERS_EVENT, OnTrainersChanged );
            MyMessenger.AddListener<string>( Tutorial.TUTORIAL_FINISHED, OnTutorialFinished );
        }

        private void UnsubscribeFromMessages() {
            EasyMessenger.Instance.RemoveListener<ITrainerManager>( TrainerManager.AVAILABLE_TRAINERS_EVENT, OnTrainersChanged );
            MyMessenger.RemoveListener<string>( Tutorial.TUTORIAL_FINISHED, OnTutorialFinished );
        }

        private void OnTrainersChanged( ITrainerManager i_trainerManager ) {
            UpdateTrainerProperties( i_trainerManager );
        }

        private void UpdateTrainerProperties( ITrainerManager i_trainerManager ) {
            CanAddTrainer = i_trainerManager.CanChangeUnitTraining( this, true );
            CanRemoveTrainer = i_trainerManager.CanChangeUnitTraining( this, false );
        }

        private void SetUnitTraining() {
            TrainingLevel = mProgress.Trainers;
        }

        private void SetUnitLevel() {
            mLevel = new Upgradeable();
            mLevel.SetPropertyToUpgrade( mModel, mData.UnitLevel );
            Level.Value = mProgress.Level;
        }

        public string GetID() {
            return mData.ID;
        }

        public float GetProgressFromTimeElapsed( TimeSpan i_timeSpan ) {
            int speed = GetProgressSpeed();
            float progressPerSecond = mData.BaseProgressPerSecond / speed;
            float progress = (float)(i_timeSpan.TotalSeconds * progressPerSecond);
            return progress;
        }

        private int GetProgressSpeed() {
            int speed = Level.Value + 1 - TrainingLevel;

            speed = Math.Max( speed, 1 );

            return speed;
        }

        public List<string> GetStats() {
            List<string> stats = new List<string>();

            foreach ( KeyValuePair<string, StatInfo> stat in mData.Stats ) {
                stats.Add( stat.Key );
            }

            return stats;
        }

        public bool HasStat( string i_stat ) {
            return mData.Stats.ContainsKey( i_stat );
        }

        public int GetBaseStat( string i_stat ) {
            float totalValue = 0f;
            StatInfo stat;
            if ( mData.Stats.TryGetValue( i_stat, out stat ) ) {
                float baseValue = stat.Base;
                totalValue = baseValue * Level.Value;
            }
            
            return (int)Math.Ceiling( totalValue );
        }

        public bool CanTrain() {
            return TrainingLevel < GetMaxTrainingLevel();
        }

        private int GetMaxTrainingLevel() {
            return Level.Value;
        }

        public void OnUpgraded() {
            UpdateTrainerPropertiesFromPlayerManager();            
        }

        private void UpdateTrainerPropertiesFromPlayerManager() {
            UpdateTrainerProperties( PlayerManager.Data.TrainerManager );
        }

        private void OnTutorialFinished( string i_tutorialName ) {
            UpdateTrainerPropertiesFromPlayerManager();
        }
    }
}