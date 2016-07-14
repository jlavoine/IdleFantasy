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
        private const string MAGIC_UNIT_NO_GUILD = GenericDataLoader.TEST_UNIT_3;

        private StatCalculator mStatCalculator;

        [SetUp]
        public void BeforeTests() {
            UnitTestUtils.LoadOfflineData();

            SetUpStatCalculator();
        }

        private void SetUpStatCalculator() {          
            UnitTestUtils.LoadMockPlayerData();

            mStatCalculator = new StatCalculator();
        }        

        static object[] CorrectTotalStatTest = {
            new object[] { UNIT_WITH_GUILD, TestUnitStats.TEST_STAT_1, 4 },
            new object[] { UNIT_WITH_GUILD, TestUnitStats.TEST_STAT_2, 3 },
            new object[] { UNIT_WITHOUT_GUILD, TestUnitStats.TEST_STAT_1, 3 },
            new object[] { UNIT_WITHOUT_GUILD, TestUnitStats.TEST_STAT_2, 3 },
            new object[] { MAGIC_UNIT_NO_GUILD, TestUnitStats.TEST_STAT_3, 2 }
        };

        [Test, TestCaseSource("CorrectTotalStatTest")]
        public void TestStatCalculator_CorrectTotalStatReturned( string i_unitID, string i_stat, int i_expectedValue ) {
            Unit unit = new Unit( GenericDataLoader.GetData<UnitData>( i_unitID ),
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
            Unit unit = new Unit( GenericDataLoader.GetData<UnitData>( i_unitID ),
                new UnitProgress() { Level = 1, Trainers = 1 },
                new ViewModel() );

            int totalStat = mStatCalculator.GetStatBonusFromSource( unit, i_stat, StatBonusSources.Guilds );

            Assert.AreEqual( i_expectedValue, totalStat );
        }

        static object[] HasStatTest = {
            new object[] { TestUnitStats.TEST_STAT_1 },
            new object[] { TestUnitStats.TEST_STAT_3 }
        };

        [Test, TestCaseSource( "HasStatTest" )]
        public void TestGetUnitsWithStat( string i_stat ) {
            List<IUnit> unitsWithStat = mStatCalculator.GetUnitsWithStat( i_stat );

            Assert.GreaterOrEqual( unitsWithStat.Count, 1 );

            foreach ( IUnit unit in unitsWithStat ) {
                Assert.True( unit.HasStat( i_stat ) );
            }
        }

        static object[] NumUnitsRequiredTest = {
            new object[] { UNIT_WITH_GUILD, TestUnitStats.TEST_STAT_1, 100, 25 },
            new object[] { UNIT_WITH_GUILD, TestUnitStats.TEST_STAT_2, 35, 12 },
            new object[] { UNIT_WITHOUT_GUILD, TestUnitStats.TEST_STAT_1, 9, 3 },
            new object[] { UNIT_WITHOUT_GUILD, TestUnitStats.TEST_STAT_2, 13, 5 },
            new object[] { MAGIC_UNIT_NO_GUILD, TestUnitStats.TEST_STAT_3, 67, 34 }
        };

        [Test, TestCaseSource( "NumUnitsRequiredTest" )]
        public void TestGetNumUnitsRequired_ReturnsCorrectValue( string i_unitID, string i_stat, int i_powerRequired, int i_expectedNumUnitsRequired ) {
            Unit unit = new Unit( GenericDataLoader.GetData<UnitData>( i_unitID ),
                new UnitProgress() { Level = 1, Trainers = 1 },
                new ViewModel() );

            int numUnitsRequired = mStatCalculator.GetNumUnitsForRequirement( unit, i_stat, i_powerRequired );

            Assert.AreEqual( i_expectedNumUnitsRequired, numUnitsRequired );
        }
    }        
}