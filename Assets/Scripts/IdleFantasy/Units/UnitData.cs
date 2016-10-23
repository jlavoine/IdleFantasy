using System.Collections.Generic;
using IdleFantasy.Data;
using MyLibrary;

namespace IdleFantasy {
    public class UnitData : GenericData {      
        public float BaseProgressPerSecond;

        public UpgradeData UnitLevel;

        public Dictionary<string, StatInfo> Stats;

        public string GetName() {
            return StringTableManager.Get( "UNIT_NAME_" + ID );
        }
    }
}