using System.Collections.Generic;

namespace IdleFantasy {
    public class BuildingData : GenericData {
        public List<string> Categories;

        public List<string> Units;

        public int Size;

        public UpgradeData LevelUpgrade;        

        public string GetName() {
            return StringTableManager.Get( "BUILDING_NAME_" + ID );
        }
    }
}