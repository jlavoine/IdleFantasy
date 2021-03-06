﻿using NUnit.Framework;
using MyLibrary;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests.Units {
    [TestFixture]
    public class TestUnitStats {
        private Unit mUnit;

        public const string TEST_STAT_1 = "TEST_STAT_1";
        public const string TEST_STAT_2 = "TEST_STAT_2";
        public const string TEST_STAT_3 = "TEST_STAT_3";
        public const string TEST_STAT_NONE = "TEST_STAT_NONE";

        static object[] HasStatSource = {
            new object[] { TEST_STAT_1, true },
            new object[] { TEST_STAT_2, true },
            new object[] { TEST_STAT_NONE, false }
        };

        static object[] RoundedStatTestSource = {
            new object[] { TEST_STAT_1, 0, 0 },
            new object[] { TEST_STAT_1, 1, 2 },
            new object[] { TEST_STAT_1, 2, 3 },
            new object[] { TEST_STAT_2, 1, 3 },
            new object[] { TEST_STAT_2, 2, 5 },
            new object[] { TEST_STAT_2, 3, 8 },
            new object[] { TEST_STAT_NONE, 0, 0 }
        };

        [SetUp]
        public void BeforeTests() {
            UnitTestUtils.LoadOfflineData();
            mUnit = new Unit( GenericDataLoader.GetData<UnitData>( GenericDataLoader.TEST_UNIT ), 
                new UnitProgress() { Level = 1, Trainers = 1 },
                new ViewModel() );
        }

        [Test]
        [TestCaseSource("HasStatSource")]
        public void HasStatReturnsExpected( string i_stat, bool i_expected ) {
            bool hasStat = mUnit.HasStat( i_stat );
        
            Assert.AreEqual( i_expected, hasStat );
        }

        [Test]
        [TestCaseSource("RoundedStatTestSource")]
        public void GetRoundedStatValue_ReturnsExpected( string i_stat, int i_unitLevel, int i_expected ) {
            mUnit.Level.Value = i_unitLevel;
            int statValue = mUnit.GetBaseStat( i_stat );

            Assert.AreEqual( i_expected, statValue );
        }
    }
}
