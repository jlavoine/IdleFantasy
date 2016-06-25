using NUnit.Framework;
using MyLibrary;
using System.Collections.Generic;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests.Units {
    [TestFixture]
    public class TestUnitModificationData {
        private Unit mUnit;
        private UnitModificationData mModData;

        private const float DEFAULT_BASE_MODIFIER = 1.5f;

        [SetUp]
        public void BeforeTests() {
            UnitTestUtils.LoadOfflineData();
            mUnit = new Unit( GenericDataLoader.GetData<UnitData>( GenericDataLoader.UNITS, GenericDataLoader.TEST_UNIT ),
                new UnitProgress() { Level = 1, Trainers = 1 },
                new ViewModel() );

            mModData = new UnitModificationData();
            mModData.BaseModifier = DEFAULT_BASE_MODIFIER;
            mModData.UnitsModified = new List<string>() { GenericDataLoader.TEST_UNIT };
            mModData.StatModified = TestUnitStats.TEST_STAT_1;
        }

        static object[] AffectsCorrectUnitsTest = {
            new object[] { GenericDataLoader.TEST_UNIT, true },
            new object[] { "Fake Unit", false },
            new object[] { "BASE_MELEE_2", false }
        };

        [Test, TestCaseSource( "AffectsCorrectUnitsTest" )]
        public void TestModificationData_AffectsCorrectUnits( string i_unitID, bool i_expectedResult ) {
            bool affects = mModData.AffectsUnit( i_unitID );

            Assert.AreEqual( i_expectedResult, affects );
        }
        
        static object[] ReturnsCorrectTotalModifierTest = {
            new object[] { 0, 0 },
            new object[] { -1, 0 },
            new object[] { 1, DEFAULT_BASE_MODIFIER },
            new object[] { 2, 2 * DEFAULT_BASE_MODIFIER }
        };

        [Test, TestCaseSource( "ReturnsCorrectTotalModifierTest" )]
        public void TestModificationData_ReturnsCorrectTotalModifier( int i_level, float i_expectedResult ) {
            float totalModifier = mModData.GetTotalModifier( i_level );

            Assert.AreEqual( i_expectedResult, totalModifier );
        }

        [Test]
        public void TestModificationData_AllUnitsAffectsAll() {
            mModData.UnitsModified = new List<string>() { UnitModificationData.ALL_KEY };

            bool affects = mModData.AffectsUnit( "Fake Unit" );

            Assert.AreEqual( true, affects );
        }

        static object[] FlatBonusTest = {
            new object[] { TestUnitStats.TEST_STAT_1, 0, 0 },
            new object[] { TestUnitStats.TEST_STAT_1 , -1, 0 },
            new object[] { TestUnitStats.TEST_STAT_1, 1, DEFAULT_BASE_MODIFIER },
            new object[] { TestUnitStats.TEST_STAT_1, 2, 2 * DEFAULT_BASE_MODIFIER },
            new object[] { TestUnitStats.TEST_STAT_NONE, 1, DEFAULT_BASE_MODIFIER }
        };

        [Test, TestCaseSource( "FlatBonusTest" )]
        public void TestModificationData_TestFlatBonuses( string i_stat, int i_level, float i_expectedValue ) {
            mModData.ModifierType = ModifierTypes.Flat;
            float bonus = mModData.GetBonus( mUnit, i_stat, i_level );

            Assert.AreEqual( i_expectedValue, bonus );
        }

        // this test relies on stat data in the test unit's file
        static object[] PercentBonusTest = {
            new object[] { TestUnitStats.TEST_STAT_1, 0, 0 },
            new object[] { TestUnitStats.TEST_STAT_1 , -1, 0 },
            new object[] { TestUnitStats.TEST_STAT_1, 1, 3 },
            new object[] { TestUnitStats.TEST_STAT_1, 2, 6 },
            new object[] { TestUnitStats.TEST_STAT_NONE, 1, 0 }
        };

        [Test, TestCaseSource("PercentBonusTest")]
        public void TestModificationData_TestPercentBonuses( string i_stat, int i_level, float i_expectedValue ) {
            mModData.ModifierType = ModifierTypes.Percent;
            float bonus = mModData.GetBonus( mUnit, i_stat, i_level );

            Assert.AreEqual( i_expectedValue, bonus );
        }
    }
}