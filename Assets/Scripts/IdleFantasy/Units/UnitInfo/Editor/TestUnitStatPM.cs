using NSubstitute;
using NUnit.Framework;

namespace IdleFantasy.UnitTests.UnitStatTests {
    [TestFixture]
    public class TestUnitStatPM {
        
        [Test]
        public void VerifyStatName_MatchesCalculator() {
            string statName = "TestStatName";
            IStatCalculator mockStatCalculator = Substitute.For<IStatCalculator>();
            mockStatCalculator.GetStatName( Arg.Any<string>() ).Returns( statName );

            UnitStatPM testPM = new UnitStatPM( Substitute.For<IUnit>(), "TestStat", mockStatCalculator );

            Assert.AreEqual( testPM.ViewModel.GetPropertyValue<string>( UnitStatPM.STAT_NAME_PROPERTY ), statName );
        }

        [Test]
        public void VerifyStatTotal_MatchesCalculator() {
            int totalValue = 111;
            IStatCalculator mockStatCalculator = Substitute.For<IStatCalculator>();
            mockStatCalculator.GetTotalStatFromUnit( Arg.Any<IUnit>(), Arg.Any<string>() ).Returns( totalValue );

            UnitStatPM testPM = new UnitStatPM( Substitute.For<IUnit>(), "TestStat", mockStatCalculator );

            Assert.AreEqual( testPM.ViewModel.GetPropertyValue<int>( UnitStatPM.STAT_TOTAL_PROPERTY ), totalValue );
        }

        [Test]
        public void VerifyStatBase_MatchesUnit() {
            int baseValue = 111;
            IUnit mockUnit = Substitute.For<IUnit>();
            mockUnit.GetBaseStat( Arg.Any<string>() ).Returns( baseValue );

            UnitStatPM testPM = new UnitStatPM( mockUnit, "TestStat", Substitute.For<IStatCalculator>() );

            Assert.AreEqual( testPM.ViewModel.GetPropertyValue<int>( UnitStatPM.STAT_BASE_PROPERTY ), baseValue );
        }

        [Test]
        public void VerifyGuildBonus_MatchesStatCalculator() {
            int bonusValue = 111;
            IStatCalculator mockStatCalculator = Substitute.For<IStatCalculator>();
            mockStatCalculator.GetStatBonusFromSource( Arg.Any<IUnit>(), Arg.Any<string>(), Arg.Any<StatBonusSources>() ).Returns( bonusValue );

            UnitStatPM testPM = new UnitStatPM( Substitute.For<IUnit>(), "TestStat", mockStatCalculator );

            Assert.AreEqual( testPM.ViewModel.GetPropertyValue<int>( UnitStatPM.STAT_GUILD_BONUS_PROPERTY ), bonusValue );
        }
    }
}
