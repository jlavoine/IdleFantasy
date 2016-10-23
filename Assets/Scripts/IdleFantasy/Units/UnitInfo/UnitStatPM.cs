using MyLibrary;

namespace IdleFantasy {
    public class UnitStatPM : PresentationModel {
        private IUnit mUnit;
        public IUnit Unit { get { return mUnit; } private set { mUnit = value; } }

        private string mStat;
        public string Stat { get { return mStat; } private set { mStat = value; } }

        private IStatCalculator mStatCalculator;

        public const string STAT_NAME_PROPERTY = "StatName";
        public const string STAT_BASE_PROPERTY = "StatBase";
        public const string STAT_GUILD_BONUS_PROPERTY = "GuildBonus";
        public const string STAT_ITEM_BONUS_PROPERTY = "ItemBonus";
        public const string STAT_ACHIEVEMENT_BONUS_PROPERTY = "AchievementBonus";
        public const string STAT_TOTAL_PROPERTY = "StatTotal";

        public UnitStatPM( IUnit i_unit, string i_stat, IStatCalculator i_calculator ) : base() {
            Unit = i_unit;
            Stat = i_stat;
            mStatCalculator = i_calculator;

            SetProperties();
        }

        private void SetProperties() {
            SetStatNameProperty();
            SetStatBaseValueProperty();
            SetStatTotalValueProperty();
            SetStatGuildBonusProperty();
            SetStatItemBonusProperty();
            SetStatAchievementBonusProperty();
        }

        private void SetStatNameProperty() {
            ViewModel.SetProperty( STAT_NAME_PROPERTY, mStatCalculator.GetStatName( Stat ) );
        }

        private void SetStatBaseValueProperty() {
            ViewModel.SetProperty( STAT_BASE_PROPERTY, Unit.GetBaseStat( Stat ) );
        }

        private void SetStatTotalValueProperty() {
            ViewModel.SetProperty( STAT_TOTAL_PROPERTY, mStatCalculator.GetTotalStatFromUnit( Unit, Stat ) );
        }

        private void SetStatGuildBonusProperty() {
            ViewModel.SetProperty( STAT_GUILD_BONUS_PROPERTY, mStatCalculator.GetStatBonusFromSource( Unit, Stat, StatBonusSources.Guilds ) );
        }

        private void SetStatItemBonusProperty() {
            ViewModel.SetProperty( STAT_ITEM_BONUS_PROPERTY, 0 );
        }

        private void SetStatAchievementBonusProperty() {
            ViewModel.SetProperty( STAT_ACHIEVEMENT_BONUS_PROPERTY, 0 );
        }
    }
}
