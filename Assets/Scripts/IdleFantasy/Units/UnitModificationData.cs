using System.Collections.Generic;
using MyLibrary;

namespace IdleFantasy {
    public class UnitModificationData  {
        public const string ALL_STATS_KEY = "All";

        public List<string> UnitsModified;
        public string StatModified;
        public ModifierTypes ModifierType;
        public float BaseModifier;        

        public float GetBonus( IUnit i_unit, string i_stat, int i_modLevel ) {
            float bonus = 0f;
            if ( AffectsUnit( i_unit ) ) {
                bonus = CalculateBonusValue( i_unit, i_stat, i_modLevel );
            }

            return bonus;
        }

        private bool AffectsUnit( IUnit i_unit ) {
            return UnitsModified.Contains( i_unit.GetID() ) || UnitsModified.Contains( ALL_STATS_KEY );
        }

        private float GetTotalModifier( int i_level ) {
            return BaseModifier * i_level;
        }

        private float CalculateBonusValue( IUnit i_unit, string i_stat, int i_modLevel ) {
            float bonus = 0;
            float totalModifier = GetTotalModifier( i_modLevel );
            int baseStatValue = i_unit.GetBaseStat( i_stat );

            switch ( ModifierType ) {
                case ModifierTypes.Flat:
                    bonus = baseStatValue * totalModifier;
                    break;
                case ModifierTypes.Percent:
                    bonus = baseStatValue + totalModifier;
                    break;
                default:
                    Messenger.Broadcast<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "No unit modification handle for " + ModifierType, "UnitModification" );
                    break;
            }

            return bonus;
        }
    }
}
