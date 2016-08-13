using MyLibrary;
using System.Collections.Generic;

namespace IdleFantasy {
    public class MapPieceData {
        public string ID;
        public MapPieceTypes PieceType;
        public int Weight;
        public NumberRestriction LevelRestriction;
        public List<MapModification> Modifications;
    }
}
