using NUnit.Framework;
using MyLibrary;
using System.Collections.Generic;
using NSubstitute;

#pragma warning disable 0414

// NOTE: The stat calculator & these tests rely on the data loaded by the offline backend, set up in the SetUp method.

namespace IdleFantasy.UnitTests.Units {
    [TestFixture]
    public class TestStatCalculator {
        private const string UNIT_WITH_GUILD = GenericDataLoader.TEST_UNIT;
        private const string UNIT_WITHOUT_GUILD = GenericDataLoader.TEST_UNIT_2;

        private StatCalculator mStatCalculator;

        [SetUp]
        public void BeforeTests() {
            UnitTestUtils.LoadOfflineData();

            GuildProgress guild_1 = new GuildProgress();
            guild_1.ID = "GUILD_1";
            guild_1.Level = 1;

            GuildProgress guild_2 = new GuildProgress();
            guild_2.ID = "GUILD_2";
            guild_2.Level = 1;

            List<Guild> guilds = new List<Guild>();
            guilds.Add( new Guild( guild_1 ) );
            guilds.Add( new Guild( guild_2 ) );

            IPlayerData mockPlayerData = NSubstitute.Substitute.For<IPlayerData>();
            mockPlayerData.Guilds.Returns( guilds );

            PlayerManager.Init( mockPlayerData );

            mStatCalculator = new StatCalculator();
        }

        static object[] CorrectTotalStatTest = {
            new object[] { UNIT_WITH_GUILD, TestUnitStats.TEST_STAT_1, 4 },
            new object[] { UNIT_WITH_GUILD, TestUnitStats.TEST_STAT_2, 3 },
            new object[] { UNIT_WITHOUT_GUILD, TestUnitStats.TEST_STAT_1, 3 },
            new object[] { UNIT_WITHOUT_GUILD, TestUnitStats.TEST_STAT_2, 3 }
        };

        [Test, TestCaseSource("CorrectTotalStatTest")]
        public void TestStatCalculator_CorrectTotalStatReturned( string i_unitID, string i_stat, int i_expectedValue ) {
            Unit unit = new Unit( GenericDataLoader.GetData<UnitData>( GenericDataLoader.UNITS, i_unitID ),
                new UnitProgress() { Level = 1, Trainers = 1 },
                new ViewModel() );

            int totalStat = mStatCalculator.GetTotalStatFromUnit( unit, i_stat );

            Assert.AreEqual( i_expectedValue, totalStat );
        }

        static object[] CorrectGuildBonusTest = {
            new object[] { UNIT_WITH_GUILD, TestUnitStats.TEST_STAT_1, 2 },
            new object[] { UNIT_WITH_GUILD, TestUnitStats.TEST_STAT_2, 0 },
            new object[] { UNIT_WITHOUT_GUILD, TestUnitStats.TEST_STAT_1, 1 },
            new object[] { UNIT_WITHOUT_GUILD, TestUnitStats.TEST_STAT_2, 0 }
        };

        [Test, TestCaseSource("CorrectGuildBonusTest")]
        public void TestStatCalculator_GuildBonusIsCorrect( string i_unitID, string i_stat, int i_expectedValue ) {
            Unit unit = new Unit( GenericDataLoader.GetData<UnitData>( GenericDataLoader.UNITS, i_unitID ),
                new UnitProgress() { Level = 1, Trainers = 1 },
                new ViewModel() );

            int totalStat = mStatCalculator.GetStatBonusFromSource( unit, i_stat, StatBonusSources.Guilds );

            Assert.AreEqual( i_expectedValue, totalStat );
        }
    }        
}