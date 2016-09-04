using System.Collections.Generic;
using System;
using MyLibrary;

namespace IdleFantasy {
    public class StatCalculator : IStatCalculator {
        private static IStatCalculator mInstance;        
        public static IStatCalculator Instance {
            get {
                if ( mInstance == null ) {
                    mInstance = new StatCalculator();
                }

                return mInstance;
            }
            set {
                mInstance = value;
            }
        }     

        public StatCalculator() {                    
        }

        public List<IUnit> GetUnitsWithStat( string i_stat ) {
            List<IUnit> unitsWithStat = new List<IUnit>();

            foreach ( Building building in PlayerManager.Data.Buildings ) {
                int unitStat = GetTotalStatFromUnit( building.Unit, i_stat );
                if ( unitStat > 0 ) {
                    unitsWithStat.Add( building.Unit );
                }
            }

            return unitsWithStat;
        }

        public int GetNumUnitsForRequirement( IUnit i_unit, string i_stat, int i_powerRequirement ) {
            int totalUnitsRequired = 0;

            int unitPower = GetTotalStatFromUnit( i_unit, i_stat );
            if ( unitPower > 0 ) {
                totalUnitsRequired = (int) Math.Ceiling( (float)i_powerRequirement / unitPower );
            } else {
                totalUnitsRequired = int.MaxValue;
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "Attempting to get units required for " + i_unit.GetID() + " for stat " + i_stat + " but that unit's power is 0!", "StatCalculation" );
            }

            return totalUnitsRequired;
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