using System.Collections.Generic;

namespace IdleFantasy {
    public class MapData {
        public string World;
        public int MapLevel;

        public MapPieceData Prefix;
        public MapPieceData Terrain;
        public MapPieceData Suffix;

        public List<MapAreaData> Areas;
    }
}
