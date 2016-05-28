using UnityEngine;

namespace IdleFantasy {
    public class TrainerAssignmentController : MonoBehaviour {

        private IUnit mUnit;

        public void Init( IUnit i_unit ) {
            mUnit = i_unit;
        }

        public void IncreaseTrainingLevel() {
            PlayerManager.Data.TrainerManager.InitiateChangeInTraining( mUnit, true );
        }

        public void DecreaseTrainingLevel() {
            PlayerManager.Data.TrainerManager.InitiateChangeInTraining( mUnit, false );
        }
    }
}
