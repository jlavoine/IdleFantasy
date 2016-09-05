using MyLibrary;

namespace IdleFantasy {
    public static class UnitKeys {
        public const string ICON = "icon_";
        public const string NAME = "UNIT_NAME_";

        public const string UNLOCK_EVENT = "UnitUnlocked";

        public static string GetIconKey( string i_unitID ) {
            return ICON + i_unitID;
        }

        public static string GetName( string i_unitID ) {
            string key = NAME + i_unitID;
            return StringTableManager.Get( key );
        }
    }
}
