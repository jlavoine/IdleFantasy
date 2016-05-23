using UnityEngine;

namespace IdleFantasy {
    public class PurchaseTrainerController : MonoBehaviour {
        ITrainerManager mTrainerManager;
        IResourceInventory mResources;

        void Start() {
            mTrainerManager = PlayerManager.Data.TrainerManager;
            mResources = (IResourceInventory) PlayerManager.Data;
        }

        public void PurchaseClicked() {
            mTrainerManager.InitiateTrainerPurchase( mResources );
        }
    }
}