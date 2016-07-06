
namespace IdleFantasy {
    public static class BuildingUtils {

        public static int GetNumUnits( IUnit i_unit ) {
            foreach ( Building building in PlayerManager.Data.Buildings ) {
                if ( building.Unit == i_unit ) {
                    return building.NumUnits;
                }
            }

            return 0;
        }
    }
}
