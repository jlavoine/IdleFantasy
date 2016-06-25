using NUnit.Framework;
using System;
using MyLibrary;
using IdleFantasy.UnitTests.Units;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests.Buildings {
    [TestFixture]
    public class BuildingStatTests {
        private Building mBuilding;

        [SetUp]
        public void BeforeTest() {
            UnitTestUtils.LoadOfflineData();

            mBuilding = BuildingUpgradeTests.GetTestBuilding();
        }

        static object[] StatsList = {
            new object[] { TestUnitStats.TEST_STAT_1 },
            new object[] { TestUnitStats.TEST_STAT_2 },
            new object[] { TestUnitStats.TEST_STAT_NONE }
        };

        [Test]
        [TestCaseSource( "StatsList" )]
        public void BuildingWithNoUnits_ReturnsNoValue( string i_stat ) {
            mBuilding.NumUnits = 0;

            long totalStat = mBuilding.GetStatTotal( i_stat );

            Assert.AreEqual( 0, totalStat );
        }

        [Test]
        public void UnknownStat_ReturnsNoValue() {
            mBuilding.NumUnits = 100;

            long totalStat = mBuilding.GetStatTotal( TestUnitStats.TEST_STAT_NONE );

            Assert.AreEqual( 0, totalStat );
        }

        static object[] Normal_ExpectedValueTest = {
            new object[] { 1, 1, TestUnitStats.TEST_STAT_1, 2 },
            new object[] { 1, 2, TestUnitStats.TEST_STAT_1, 3 },
            new object[] { 10, 1, TestUnitStats.TEST_STAT_1, 20 },
            new object[] { 100, 1, TestUnitStats.TEST_STAT_1, 200 },
            new object[] { 100, 2, TestUnitStats.TEST_STAT_1, 300 },
            new object[] { 3129, 1, TestUnitStats.TEST_STAT_1, 6258 },
            new object[] { 1, 1, TestUnitStats.TEST_STAT_2, 3 },
            new object[] { 7, 1, TestUnitStats.TEST_STAT_2, 21 },
            new object[] { 7, 3, TestUnitStats.TEST_STAT_2, 56 },
            new object[] { 1173, 1, TestUnitStats.TEST_STAT_2, 3519 },            
        };

        [Test]
        [TestCaseSource( "Normal_ExpectedValueTest" )]
        public void GetStatTotal_ReturnsExpectedValue( int i_unitCount, int i_unitLevel, string i_stat, long i_expectedValue ) {
            mBuilding.NumUnits = i_unitCount;
            mBuilding.Unit.Level.Value = i_unitLevel;

            long totalStat = mBuilding.GetStatTotal( i_stat );

            Assert.AreEqual( i_expectedValue, totalStat );
        }
    }
}