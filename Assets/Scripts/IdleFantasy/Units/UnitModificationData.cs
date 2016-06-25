using System;
using System.Collections.Generic;
using MyLibrary;

namespace IdleFantasy {
    public class UnitModificationData  {
        public const string ALL_KEY = "All";

        public List<string> UnitsModified;
        public string StatModified;
        public ModifierTypes ModifierType;
        public float BaseModifier;        

        public float GetBonus( IUnit i_unit, string i_stat, int i_modLevel ) {
            float bonus = 0f;
            if ( ModifiesStat( i_stat ) && AffectsUnit( i_unit.GetID() ) ) {
                bonus = CalculateBonusValue( i_unit, i_stat, i_modLevel );
            }

            return bonus;
        }

        public bool AffectsUnit( string i_unitID ) {
            return UnitsModified.Contains( i_unitID ) || UnitsModified.Contains( ALL_KEY );
        }

        public bool ModifiesStat( string i_stat ) {
            return StatModified == i_stat;
        }

        public float GetTotalModifier( int i_level ) {
            i_level = Math.Max( i_level, 0 );

            return BaseModifier * i_level;
        }

        private float CalculateBonusValue( IUnit i_unit, string i_stat, int i_modLevel ) {
            float bonus = 0;
            float totalModifier = GetTotalModifier( i_modLevel );
            int baseStatValue = i_unit.GetBaseStat( i_stat );

            switch ( ModifierType ) {
                case ModifierTypes.Flat:
                    bonus = totalModifier;
                    break;
                case ModifierTypes.Percent:
                    bonus = baseStatValue * totalModifier;
                    break;
                default:
                    MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Error, "No unit modification handle for " + ModifierType, "UnitModification" );
                    break;
            }

            return bonus;
        }
    }
}
