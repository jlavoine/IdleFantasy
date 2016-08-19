using System.Collections.Generic;

namespace IdleFantasy {
    public static class MapModificationHelper {
        public static float GetModAmount( this List<MapModification> i_list, string i_key ) {
            foreach ( MapModification mapMod in i_list ) {
                if ( mapMod.Key == i_key ) {
                    return mapMod.Amount;
                }
            }

            return 0f;
        }
    }
}
