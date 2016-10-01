using UnityEngine;
using MyLibrary;

namespace IdleFantasy {
    public class PurchaseTrainerController : MonoBehaviour {
        public const string BUY_TRAINER_MESSAGE = "TrainerPurchased"; // for tutorial

        ITrainerManager mTrainerManager;
        IResourceInventory mResources;

        void Start() {
            mTrainerManager = PlayerManager.Data.TrainerManager;
            mResources = (IResourceInventory) PlayerManager.Data;
        }

        public void PurchaseClicked() {
            MyMessenger.Send( BUY_TRAINER_MESSAGE );

            mTrainerManager.InitiateTrainerPurchase( mResources );

            BackendManager.Backend.MakeTrainerPurchase();
        }
    }
}