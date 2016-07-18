
namespace IdleFantasy {
    public interface IBuildingUtils {
        int GetNumUnits( IUnit i_unit );
        void AlterUnitCount( string i_unitID, int i_amount );
    }
}
