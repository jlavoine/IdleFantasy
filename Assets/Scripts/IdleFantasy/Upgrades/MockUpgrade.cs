using MyLibrary;
using System;
using System.Collections.Generic;

namespace IdleFantasy {
    public class MockUpgrade : IUpgradeable {
        public int MaxLevel {
            get {
                throw new NotImplementedException();
            }
        }

        public Dictionary<string, int> ResourcesToUpgrade {
            get {
                throw new NotImplementedException();
            }
        }

        public UpgradeData UpgradeData {
            get {
                throw new NotImplementedException();
            }
        }

        public int Value {
            get {
                throw new NotImplementedException();
            }

            set {
                throw new NotImplementedException();
            }
        }

        public event UpgradeComplete UpgradeCompleteEvent;

        public bool CanAffordUpgrade( IResourceInventory i_inventory ) {
            throw new NotImplementedException();
        }

        public bool CanUpgrade( IResourceInventory i_inventory ) {
            throw new NotImplementedException();
        }

        public void ChargeForUpgrade( IResourceInventory i_inventory ) {
            throw new NotImplementedException();
        }

        public int GetUpgradeCostForResource( string i_resource ) {
            throw new NotImplementedException();
        }

        public void InitiateUpgrade( IResourceInventory i_inventory ) {
            throw new NotImplementedException();
        }

        public bool IsAtMaxLevel() {
            throw new NotImplementedException();
        }

        public void SetPropertyToUpgrade( ViewModel i_model, UpgradeData i_data ) {
            throw new NotImplementedException();
        }

        public void Upgrade() {
            throw new NotImplementedException();
        }
    }
}