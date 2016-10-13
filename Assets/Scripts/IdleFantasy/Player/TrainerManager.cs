using System.Collections.Generic;
using System;
using MyLibrary;

namespace IdleFantasy {
    public class TrainerManager : ITrainerManager {
        public const string TOTAL_TRAINERS = "TotalTrainers";
        public const string CURRENT_TRAINERS = "CurrentTrainers";
        public const string NEXT_TRAINER_COST = "NextTrainerCost";
        public const string CAN_AFFORD_TRAINER = "CanAffordNextTrainer";
        public const string NORMAL_TRAINERS = "Normal";

        public const string AVAILABLE_TRAINERS_EVENT = "AvailableTrainersChanged";

        public const string STARTING_COST_KEY = "TrainerStartingCost";
        public const string TRAINER_UPGRADE_COEFFICIENT = "TrainerUpgradeCoefficient";

        private Dictionary<string, int> mTrainers = new Dictionary<string, int>();
        private Dictionary<string, int> mTrainerAssignments = new Dictionary<string, int>();

        private ViewModel mPlayerModel;

        public int AvailableTrainers {
            get { return mPlayerModel.GetPropertyValue<int>( CURRENT_TRAINERS ); }
            set {
                if ( value < 0 ) {
                    value = 0;
                }
                else if ( value > TotalTrainers ) {
                    value = TotalTrainers;
                }

                mPlayerModel.SetProperty( CURRENT_TRAINERS, value );
                EasyMessenger.Instance.Send( AVAILABLE_TRAINERS_EVENT, (ITrainerManager) this );
            }
        }

        public int NextTrainerCost {
            get { return mPlayerModel.GetPropertyValue<int>( NEXT_TRAINER_COST ); }
            set { mPlayerModel.SetProperty( NEXT_TRAINER_COST, value ); }
        }

        public bool CanAfford {
            get { return mPlayerModel.GetPropertyValue<bool>( CAN_AFFORD_TRAINER ); }
            set { mPlayerModel.SetProperty( CAN_AFFORD_TRAINER, value ); }
        }

        public int TotalTrainers {
            get { return mPlayerModel.GetPropertyValue<int>( TOTAL_TRAINERS ); }
            set {
                if ( value < 0 ) {
                    value = 0;
                }

                mPlayerModel.SetProperty( TOTAL_TRAINERS, value );
            }
        }

        public TrainerManager( ViewModel i_playerModel, TrainerSaveData i_trainerData, Dictionary<string, UnitProgress> i_unitProgress ) {
            mTrainers = i_trainerData.TrainerCounts;
            mPlayerModel = i_playerModel;

            RefreshTrainerUI( i_unitProgress );

            SubscribeToMessages();
        }

        private void RefreshTrainerUI( Dictionary<string, UnitProgress> i_unitProgress ) {
            TotalTrainers = GetTotalTrainers();
            AvailableTrainers = GetAvailableTrainers( i_unitProgress );
            UpdateNextTrainerCost();
            UpdateCanAffordNextTrainer();
        }

        public void Dispose() {
            UnsubscribeFromMessages();
        }

        private void SubscribeToMessages() {
            EasyMessenger.Instance.AddListener<string, int>( PlayerData.INVENTORY_CHANGED_EVENT, OnInventoryChanged );
            EasyMessenger.Instance.AddListener( MapKeys.WORLD_RESET_SUCCESS, OnWorldReset );
        }

        private void UnsubscribeFromMessages() {
            EasyMessenger.Instance.RemoveListener<string, int>( PlayerData.INVENTORY_CHANGED_EVENT, OnInventoryChanged );
            EasyMessenger.Instance.RemoveListener( MapKeys.WORLD_RESET_SUCCESS, OnWorldReset );
        }

        private void OnInventoryChanged( string i_resourceType, int i_newValue ) {
            UpdateCanAffordNextTrainer();
        }

        private void OnWorldReset() {
            ResetAllTrainerAssignments( PlayerManager.Data.UnitProgress );
            mTrainers[NORMAL_TRAINERS] = 0;

            RefreshTrainerUI( PlayerManager.Data.UnitProgress );
        }

        private void UpdateCanAffordNextTrainer() {
            // FIXME: Did this because tests were failing...need a better way
            if ( PlayerManager.Data is IResourceInventory ) {
                CanAfford = CanAffordTrainerPurchase( (IResourceInventory) PlayerManager.Data );
            }
        }

        private int GetTotalTrainers() {
            int totalTrainers = 0;
            foreach ( KeyValuePair<string, int> trainerPair in mTrainers ) {
                totalTrainers += trainerPair.Value;
            }

            return totalTrainers;
        }

        private int GetAvailableTrainers( Dictionary<string, UnitProgress> i_unitProgress ) {
            int totalAssignedTrainers = GetTotalAssignedTrainers( i_unitProgress );

            if ( totalAssignedTrainers > TotalTrainers ) {
                ResetAllTrainerAssignments( i_unitProgress );
                return TotalTrainers;
            } else {
                return TotalTrainers - totalAssignedTrainers;
            }
        }

        private void ResetAllTrainerAssignments( Dictionary<string, UnitProgress> i_unitProgress ) {
            AvailableTrainers = 0;

            foreach ( KeyValuePair<string, UnitProgress> trainerAssignment in i_unitProgress ) {
                i_unitProgress[trainerAssignment.Key].Trainers = 0;
            }
        }

        public int GetTotalAssignedTrainers( Dictionary<string, UnitProgress> i_unitProgress ) {
            int totalAssignedTrainers = 0;

            foreach ( KeyValuePair<string, UnitProgress> trainerAssignment in i_unitProgress ) {
                totalAssignedTrainers += trainerAssignment.Value.Trainers;
            }

            return totalAssignedTrainers;
        }

        public int GetAssignedTrainers( string i_id ) {
            int assignedTrainers = 0;

            mTrainerAssignments.TryGetValue( i_id, out assignedTrainers );

            return assignedTrainers;
        }

        public int GetTotalTrainersOfType( string i_type ) {
            int currentTrainers = 0;
            mTrainers.TryGetValue( i_type, out currentTrainers );
            if ( currentTrainers < 0 )
                currentTrainers = 0;

            return currentTrainers;
        }

        public int GetNextTrainerCost() {
            int totalNormalTrainers = GetTotalTrainersOfType( NORMAL_TRAINERS );
            int trainerStartingCost = Constants.GetConstant<int>( STARTING_COST_KEY );
            double trainerUpgradeCoefficient = Constants.GetConstant<double>( TRAINER_UPGRADE_COEFFICIENT );
            int nextCost = (int)Math.Ceiling( (trainerStartingCost * Math.Pow( trainerUpgradeCoefficient, totalNormalTrainers ) ) );
            
            return nextCost;
        }

        private void UpdateNextTrainerCost() {
            NextTrainerCost = GetNextTrainerCost();
        }

        public void InitiateTrainerPurchase( IResourceInventory i_inventory ) {
            if ( CanAffordTrainerPurchase( i_inventory ) ) {
                ChargeForTrainerPurchase( i_inventory );
                AddTrainer( NORMAL_TRAINERS, 1 );
                UpdateNextTrainerCost();
                UpdateCanAffordNextTrainer();
            }
        }

        public bool CanAffordTrainerPurchase( IResourceInventory i_inventory ) {
            int cost = GetNextTrainerCost();
            bool canTrain = i_inventory.HasEnoughResources( VirtualCurrencies.GOLD, cost );
            return canTrain;
        }

        public void ChargeForTrainerPurchase( IResourceInventory i_inventory ) {
            int cost = GetNextTrainerCost();
            i_inventory.SpendResources( VirtualCurrencies.GOLD, cost );
        }

        public void AddTrainer( string i_type, int i_count ) {
            int numTrainers = 0;
            mTrainers.TryGetValue( i_type, out numTrainers );
            numTrainers += i_count;
            mTrainers[i_type] = numTrainers;

            TotalTrainers += i_count;
            AvailableTrainers += i_count;
        }

        public void InitiateChangeInTraining( IUnit i_unit, bool i_isTraining ) {
            if ( CanChangeUnitTraining( i_unit, i_isTraining ) ) {
                ChangeUnitTrainingLevel( i_unit, i_isTraining );
                ChangeAvailableTrainers( i_unit, i_isTraining );                
            }
        }

        public bool CanChangeUnitTraining( IUnit i_unit, bool i_isTraining ) {
            if ( i_isTraining ) {
                return HasTrainersAvailable( i_unit ) && i_unit.CanTrain();                
            }
            else {
                return i_unit.TrainingLevel > 0;
            }
        }

        private bool HasTrainersAvailable( IUnit i_unit ) {
            int costInTrainers = GetTrainersToTrainUnit( i_unit );
            return AvailableTrainers >= costInTrainers;
        }

        public int GetTrainersToTrainUnit( IUnit i_unit ) {
            return 1;// i_unit.TrainingLevel + 1;
        }

        public void ChangeAvailableTrainers( IUnit i_unit, bool i_isTraining ) {
            int trainerCost = GetTrainersToTrainUnit( i_unit );
            int change = i_isTraining ? trainerCost : -trainerCost;
            AvailableTrainers -= change;
        }

        public void ChangeUnitTrainingLevel( IUnit i_unit, bool i_isTraining ) {
            int change = i_isTraining ? 1 : -1;
            i_unit.TrainingLevel += change;
        }
    }
}