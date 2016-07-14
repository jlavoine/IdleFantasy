
namespace IdleFantasy {
    public class BuildingUtils : IBuildingUtils {

        public int GetNumUnits( IUnit i_unit ) {
            foreach ( Building building in PlayerManager.Data.Buildings ) {
                if ( building.Unit == i_unit ) {
                    return building.NumUnits;
                }
            }

            return 0;
        }

        public void AlterUnitCount( IUnit i_unit, int i_amount ) {
            foreach ( Building building in PlayerManager.Data.Buildings ) {
                if ( building.Unit == i_unit ) {
                    building.NumUnits += i_amount;
                }
            }
        }
    }
}
