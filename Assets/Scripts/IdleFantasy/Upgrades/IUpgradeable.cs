using MyLibrary;
using System.Collections.Generic;

namespace IdleFantasy {
    public interface IUpgradeable {       
        UpgradeData UpgradeData { get; }

        int MaxLevel { get; }
        int Value { get; set; }        

        Dictionary<string, int> ResourcesToUpgrade { get; }

        event UpgradeComplete UpgradeCompleteEvent;

        void SetPropertyToUpgrade( ViewModel i_model, UpgradeData i_data );
        void InitiateUpgradeWithResources( IResourceInventory i_inventory  );
        void ChargeForUpgrade( IResourceInventory i_inventory  );
        void Upgrade();        

        bool CanUpgrade( IResourceInventory i_inventory  );        
        bool CanAffordUpgrade( IResourceInventory i_inventory  );
        bool IsAtMaxLevel();

        int GetUpgradeCostForResource( string i_resource );

        #region Points
        int Points { get; set; }
        float Progress { get; set; }
        #endregion
    }
}