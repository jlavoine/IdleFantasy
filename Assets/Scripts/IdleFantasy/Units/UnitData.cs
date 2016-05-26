using System.Collections.Generic;
using IdleFantasy.Data;

namespace IdleFantasy {
    public class UnitData : GenericData {      
        public float BaseProgressPerSecond;

        public UpgradeData UnitLevel;

        public Dictionary<string, StatInfo> Stats;
    }
}