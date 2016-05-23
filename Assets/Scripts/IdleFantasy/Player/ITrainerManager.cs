
namespace IdleFantasy {
    public interface ITrainerManager {
        int AvailableTrainers {
            get;
            set;
        }

        int TotalTrainers {
            get;
            set;
        }

        int GetTotalTrainersOfType( string i_type );

        int GetNextTrainerCost();
        void InitiateTrainerPurchase( IResourceInventory i_inventory );
        bool CanAffordTrainerPurchase( IResourceInventory i_inventory );
        void ChargeForTrainerPurchase( IResourceInventory i_inventory );
        void AddTrainer( string i_type, int i_count );

        void InitiateChangeInTraining( IUnit i_unit, bool i_isTraining );
        bool CanChangeUnitTraining( IUnit i_unit, bool i_isTraining );
        int GetTrainersToTrainUnit( IUnit i_unit );

        void ChangeAvailableTrainers( IUnit i_unit, bool i_isTraining );
        void ChangeUnitTrainingLevel( IUnit i_unit, bool i_isTraining );
    }
}