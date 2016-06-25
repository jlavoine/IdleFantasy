using System.Collections.Generic;
using System;
using MyLibrary;

namespace IdleFantasy {
    public class StatCalculator {

        public StatCalculator() {                    
        }

        public int GetTotalStatFromUnit( IUnit i_unit, string i_stat ) {
            int totalStat = i_unit.GetBaseStat( i_stat );

            foreach ( StatBonusSources source in Enum.GetValues( typeof( StatBonusSources ) ) ) {
                totalStat += GetStatBonusFromSource( i_unit, i_stat, source );
            }
                        
            return totalStat;
        }

        public int GetStatBonusFromSource( IUnit i_unit, string i_stat, StatBonusSources i_source ) {
            int bonus = 0;

            switch ( i_source ) {
                case StatBonusSources.Guilds:
                    bonus = GetGuildBonus( i_unit, i_stat );
                    break;
                default:
                    MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "No stat calculation for source: " + i_source.ToString(), "UnitModification" );
                    break;
            }

            return bonus;
        }

        private int GetGuildBonus( IUnit i_unit, string i_stat ) {
            float bonus = 0f;

            List<Guild> guilds = PlayerManager.Data.Guilds;
            foreach ( Guild guild in guilds ) {
                bonus += guild.GetUnitStatBonus( i_unit, i_stat );
            }

            return (int) Math.Ceiling( bonus );
        }
    }
}