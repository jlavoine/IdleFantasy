using MyLibrary;
using System.Collections.Generic;

namespace IdleFantasy {
    public interface IUpgradeable {
        void SetPropertyToUpgrade( ViewModel i_model, UpgradeData i_data );

        int Level {
            get;
            set;
        }

        int MaxLevel {
            get;
        }

        Dictionary<string, int> ResourcesToUpgrade {
            get;
        }

        void InitiateUpgrade( IResourceInventory i_inventory  );
        void ChargeForUpgrade( IResourceInventory i_inventory  );
        void Upgrade();        

        bool CanUpgrade( IResourceInventory i_inventory  );        
        bool CanAffordUpgrade( IResourceInventory i_inventory  );
        bool IsAtMaxLevel();
        int GetUpgradeCostForResource( string i_resource );
    }
}