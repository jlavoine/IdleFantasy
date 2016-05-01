using System.Collections.Generic;
using MyLibrary;

namespace IdleFantasy {
    public class SimpleBuildingData : GenericData {
        public List<string> Categories;

        public string Unit;

        public int StartingSize;

        public int MaxLevel;

        public int StartingUpgradeCost;

        public string GetName() {
            return StringTableManager.Get( "BUILDING_NAME_" + ID );
        }
    }
}