using MyLibrary;

namespace IdleFantasy {
    public class MapName {
        public MapPieceData Prefix;
        public MapPieceData Terrain;
        public MapPieceData Suffix;

        public string GetStringName() {
            string nameKey = "_NAME";
            string prefixString = StringTableManager.Get( Prefix.ID + nameKey );
            string terrainString = StringTableManager.Get( Terrain.ID + nameKey );
            string suffixString = StringTableManager.Get( Suffix.ID + nameKey );

            string name = string.Format( "{0} {1} {2}", prefixString, terrainString, suffixString );
            return name;
        }
    }
}
