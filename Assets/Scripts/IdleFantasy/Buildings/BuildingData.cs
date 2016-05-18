using System.Collections.Generic;
using MyLibrary;

namespace IdleFantasy {
    public class BuildingData : GenericData {
        public List<string> Categories;

        public string Unit;

        public int StartingSize;

        public UpgradeData Level;        

        public string GetName() {
            return StringTableManager.Get( "BUILDING_NAME_" + ID );
        }
    }
}