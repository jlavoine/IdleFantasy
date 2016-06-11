using System;

namespace IdleFantasy {
    public interface IBuilding {
        int GetLevel();
        int GetNumUnits();
        IUnit GetUnit();
        float GetUnitProgress();
        void SetUnit( IUnit i_unit );
        int GetCapacity();

        long GetStatTotal( string i_stat );

        void Tick( TimeSpan i_timeSpan );
        void AddUnitsFromProgress( int i_numUnits );

        void InitiateUpgrade( IResourceInventory i_inventory );
        bool CanUpgrade( IResourceInventory i_inventory );
        void Update();
    }
}
