using System.Collections.Generic;

namespace IdleFantasy {
    public class MapData {
        public string World;

        public int MapLevel;

        public MapName Name;

        public List<MapName> UpcomingMaps;
        public List<MapAreaData> Areas;
    }
}
