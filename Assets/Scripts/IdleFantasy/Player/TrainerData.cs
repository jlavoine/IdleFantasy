using UnityEngine;
using System.Collections.Generic;
using System;
using MyLibrary;

namespace IdleFantasy {
    public class TrainerData : ITrainerData {
        public const string TOTAL_TRAINERS = "TotalTrainers";
        public const string CURRENT_TRAINERS = "CurrentTrainers";
        public const string NORMAL_TRAINERS = "NormalTrainers";

        public const string STARTING_COST_KEY = "TrainerStartingCost";

        private Dictionary<string, int> mTrainers = new Dictionary<string, int>();

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
            }
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

        public TrainerData( ViewModel i_playerModel, Dictionary<string, int> i_trainerData ) {
            mTrainers = i_trainerData;
            mPlayerModel = i_playerModel;

            TotalTrainers = GetTotalTrainers();
            AvailableTrainers = TotalTrainers;
            // TODO: need to use save data somewhere here
        }

        private int GetTotalTrainers() {
            int totalTrainers = 0;
            foreach ( KeyValuePair<string, int> trainerPair in mTrainers ) {
                totalTrainers += trainerPair.Value;
            }

            return totalTrainers;
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
            int nextCost = (totalNormalTrainers + 1) * trainerStartingCost;

            return nextCost;
        }

        public void InitiateTrainerPurchase( IResourceInventory i_inventory ) {
            if ( CanAffordTrainerPurchase( i_inventory ) ) {
                ChargeForTrainerPurchase( i_inventory );

                AddTrainer( NORMAL_TRAINERS, 1 );
            }
        }

        public bool CanAffordTrainerPurchase( IResourceInventory i_inventory ) {
            int cost = GetNextTrainerCost();
            bool canTrain = i_inventory.HasEnoughResources( NormalInventory.GOLD, cost );
            return canTrain;
        }

        public void ChargeForTrainerPurchase( IResourceInventory i_inventory ) {
            int cost = GetNextTrainerCost();
            i_inventory.SpendResources( NormalInventory.GOLD, cost );
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
                ChangeAvailableTrainers( i_unit, i_isTraining );

                ChangeUnitTrainingLevel( i_unit, i_isTraining );
            }
        }

        public bool CanChangeUnitTraining( IUnit i_unit, bool i_isTraining ) {
            if ( i_isTraining ) {
                int costInTrainers = GetTrainersToTrainUnit( i_unit );
                return AvailableTrainers >= costInTrainers;
            }
            else {
                return i_unit.TrainingLevel > 0;
            }
        }

        public int GetTrainersToTrainUnit( IUnit i_unit ) {
            return i_unit.TrainingLevel + 1;
        }

        public void ChangeAvailableTrainers( IUnit i_unit, bool i_isTraining ) {
            int levelsToChangeBy = i_isTraining ? i_unit.TrainingLevel : i_unit.TrainingLevel - 1;
            int change = i_isTraining ? levelsToChangeBy : -levelsToChangeBy;
            AvailableTrainers -= change;
        }

        public void ChangeUnitTrainingLevel( IUnit i_unit, bool i_isTraining ) {
            int change = i_isTraining ? 1 : -1;
            i_unit.TrainingLevel += change;
        }
    }
}