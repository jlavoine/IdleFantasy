using System.Collections.Generic;

namespace IdleFantasy {
    public class BuildingData {
        public string ID;

        public List<string> Categories;

        public List<string> Units;

        public int Size;
        public int MaxLevel;

        public Dictionary<string, int> ResourcesToUpgrade;

        public string GetName() {
            return StringTableManager.Get( "BUILDING_NAME_" + ID );
        }
    }
}