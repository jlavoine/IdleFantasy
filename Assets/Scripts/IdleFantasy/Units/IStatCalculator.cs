using System.Collections.Generic;

namespace IdleFantasy {
    public interface IStatCalculator {
        List<IUnit> GetUnitsWithStat( string i_stat );
        int GetNumUnitsForRequirement( IUnit i_unit, string i_stat, int i_powerRequirement );
        int GetTotalStatFromUnit( IUnit i_unit, string i_stat );
        int GetStatBonusFromSource( IUnit i_unit, string i_stat, StatBonusSources i_source );
    }
}
