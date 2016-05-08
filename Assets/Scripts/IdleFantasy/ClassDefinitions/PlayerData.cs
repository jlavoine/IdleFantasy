using System;
using System.Collections.Generic;
using MyLibrary;
using Newtonsoft.Json;

namespace IdleFantasy {
    public class PlayerData : IPlayerData, IResourceInventory {
        public const string BUILDING_PROGRESS = "BuildingProgress";
        public const string UNIT_PROGRESS = "UnitProgress";

        public Dictionary<string, UnitProgress> UnitProgress;
        public Dictionary<string, int> UnitTrainingLevels;

        public Dictionary<string, int> Trainers;

        public Dictionary<string, BuildingProgress> BuildingProgress;

        private Dictionary<string, int> mInventory = new Dictionary<string, int>();

        private ViewModel mModel;

        private IBackend mBackend;
        
        public void Init( IBackend i_backend ) {
            mBackend = i_backend;

            mBackend.GetPlayerData( BUILDING_PROGRESS, (jsonData) => {
                BuildingProgress = JsonConvert.DeserializeObject<Dictionary<string, BuildingProgress>>( jsonData );
            } );

            mBackend.GetPlayerData( UNIT_PROGRESS, ( jsonData ) => {
                UnitProgress = JsonConvert.DeserializeObject<Dictionary<string, UnitProgress>>( jsonData );
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
            get { return mModel.GetPropertyValue<int>( "Gold" ); }
            set {
                mModel.SetProperty( "Gold", value );

                if ( !mInventory.ContainsKey( "Gold" ) ) {
                    mInventory.Add( "Gold", value );
                }
                else {
                    mInventory["Gold"] = value;
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