using System.Collections.Generic;

namespace IdleFantasy {
    public class MapData : IMapData {
        public string World;

        public int MapLevel;
        public int MapSize;

        public MapName Name;

        public List<MapModification> AllModifications;
        public List<MapName> UpcomingMaps;
        public List<MapAreaData> Areas;

        public List<MapName> GetUpcomingMaps() {
            return UpcomingMaps;
        }

        public int GetLevel() {
            return MapLevel;
        }
    }
}
