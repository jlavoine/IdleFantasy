using System.Collections.Generic;

namespace IdleFantasy {
    public abstract class BaseUnitModifierData : GenericData {
        public UpgradeData Level;

        public List<UnitModificationData> UnitModifications;
    }
}
