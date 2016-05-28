using System;
using System.Collections.Generic;
using MyLibrary;
using Newtonsoft.Json;

namespace IdleFantasy {
    public class PlayerData : IPlayerData, IResourceInventory {
        public const string BUILDING_PROGRESS = "BuildingsProgress";
        public const string UNIT_PROGRESS = "UnitsProgress";
        public const string TRAINER_SAVE_DATA = "TrainerSaveData";

        public Dictionary<string, UnitProgress> UnitProgress;
        public Dictionary<string, int> UnitTrainingLevels;

        public Dictionary<string, BuildingProgress> BuildingProgress;

        private TrainerSaveData mTrainerSaveData;
        public ITrainerManager TrainerManager;

        private Dictionary<string, int> mInventory = new Dictionary<string, int>();

        private ViewModel mModel;

        private IBasicBackend mBackend;
        
        public void Init( IBasicBackend i_backend ) {
            mBackend = i_backend;
            mModel = new ViewModel();

            mBackend.GetPlayerData( BUILDING_PROGRESS, (jsonData) => {
                BuildingProgress = JsonConvert.DeserializeObject<Dictionary<string, BuildingProgress>>( jsonData );
            } );

            mBackend.GetPlayerData( UNIT_PROGRESS, ( jsonData ) => {
                UnitProgress = JsonConvert.DeserializeObject<Dictionary<string, UnitProgress>>( jsonData );
            } );

            mBackend.GetPlayerData( TRAINER_SAVE_DATA, ( jsonData ) => {
                mTrainerSaveData = JsonConvert.DeserializeObject<TrainerSaveData>( jsonData );
                TrainerManager = new TrainerManager( mModel, mTrainerSaveData, UnitProgress );
            } );

            mBackend.GetVirtualCurrency( VirtualCurrencies.GOLD, ( numGold ) => {
                Gold = numGold;
            } );
        }

        public object GetData( string i_key ) {
            switch ( i_key ) {
                case BUILDING_PROGRESS:
                    return BuildingProgress;
                case UNIT_PROGRESS:
                    return UnitProgress;
                default:
                    return null;
            }
        }

        public int Gold {
            get { return mModel.GetPropertyValue<int>( VirtualCurrencies.GOLD ); }
            set {
                mModel.SetProperty( VirtualCurrencies.GOLD, value );

                if ( !mInventory.ContainsKey( VirtualCurrencies.GOLD ) ) {
                    mInventory.Add( VirtualCurrencies.GOLD, value );
                }
                else {
                    mInventory[VirtualCurrencies.GOLD] = value;
                }
            }
        }

        public ViewModel GetViewModel() {
            return mModel;
        }

        public int GetResourceCount( string i_resource ) {
            if ( mInventory.ContainsKey( i_resource ) ) {
                return mInventory[i_resource];
            }
            else {
                mInventory[i_resource] = 0;
                return 0;
            }
        }

        public bool HasEnoughResources( string i_resource, int i_count ) {
            int amountOfResource = GetResourceCount( i_resource );
            bool hasEnough = amountOfResource >= i_count;
            return hasEnough;
        }

        public void SpendResources( string i_resource, int i_count ) {
            int amountOfResource = GetResourceCount( i_resource );
            int remainingValue = Math.Max( amountOfResource - i_count, 0 );
            mInventory[i_resource] = remainingValue;

            UpdateInventoryData();
        }

        public void UpdateInventoryData() {
            foreach ( KeyValuePair<string, int> inventoryItem in mInventory ) {
                mModel.SetProperty( inventoryItem.Key, inventoryItem.Value );
            }
        }
    }
}