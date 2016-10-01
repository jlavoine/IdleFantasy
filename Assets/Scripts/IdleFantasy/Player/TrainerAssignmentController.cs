using UnityEngine;
using MyLibrary;

namespace IdleFantasy {
    public class TrainerAssignmentController : MonoBehaviour {
        public const string ADD_TRAINER_MESSAGE = "TrainerAdded"; // for tutorial

        private IUnit mUnit;

        public void Init( IUnit i_unit ) {
            mUnit = i_unit;
        }

        public void IncreaseTrainingLevel() {
            MyMessenger.Send( ADD_TRAINER_MESSAGE );

            PlayerManager.Data.TrainerManager.InitiateChangeInTraining( mUnit, true );

            BackendManager.Backend.ChangeAssignedTrainers( mUnit.GetID(), 1 );
        }

        public void DecreaseTrainingLevel() {
            PlayerManager.Data.TrainerManager.InitiateChangeInTraining( mUnit, false );

            BackendManager.Backend.ChangeAssignedTrainers( mUnit.GetID(), -1 );
        }
    }
}
