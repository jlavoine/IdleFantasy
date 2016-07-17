using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;
using IdleFantasy.UnitTests.Units;
using NSubstitute;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class TaskUnitSelectionTest {

        private Dictionary<IUnit, int> mPromisedUnits;
        private Dictionary<int, MissionTaskProposal> mTaskProposals;

        private TaskUnitSelection mTestSelection;
        private const int TEST_TASK_INDEX = 1;
        private const int POWER_REQUIREMENT = 100;
        private const string TEST_STAT = TestUnitStats.TEST_STAT_1;
        private IUnit mUnit;

        private IPlayerData mPlayerData;

        [SetUp]
        public void BeforeTest() {
            UnitTestUtils.LoadOfflineData();
            mPlayerData = UnitTestUtils.LoadMockPlayerData();

            mPromisedUnits = new Dictionary<IUnit, int>();
            mTaskProposals = new Dictionary<int, MissionTaskProposal>();
            mUnit = new MockUnit( 100 );
            mTestSelection = new TaskUnitSelection( mUnit, 
                new MissionTaskData() { Index = TEST_TASK_INDEX, StatRequirement = TEST_STAT, PowerRequirement = POWER_REQUIREMENT }, 
                mPromisedUnits, mTaskProposals );

            SetPlayerDataToNotEnoughUnits();
        }

        [Test]
        public void VerifyVariables_FromCreatedTaskUnitSelection() {
            Assert.AreEqual( mTestSelection.PowerRequirement, POWER_REQUIREMENT );
            Assert.AreEqual( mTestSelection.Stat, TEST_STAT );
            Assert.AreEqual( mTestSelection.Unit, mUnit );
        }

        [Test]
        public void VerifyProperties_FromCreatedTaskUnitSelection() {
            int unitsRequired = StatCalculator.Instance.GetNumUnitsForRequirement( mUnit, TEST_STAT, POWER_REQUIREMENT );
            Color color = Constants.GetConstant<Color>( ConstantKeys.NOT_ENOUGH_UNITS_COLOR );

            Assert.AreEqual( false, mTestSelection.ViewModel.GetPropertyValue<bool>( MissionKeys.IS_UNIT_SELECTABLE ) );
            Assert.AreEqual( color, mTestSelection.ViewModel.GetPropertyValue<Color>( MissionKeys.NUM_UNITS_FOR_TASK_COLOR ) );
            Assert.AreEqual( unitsRequired, mTestSelection.ViewModel.GetPropertyValue<int>( MissionKeys.NUM_UNITS_FOR_TASK ) );
            Assert.AreEqual( mUnit.GetID(), mTestSelection.ViewModel.GetPropertyValue<string>( MissionKeys.UNIT_FOR_TASK ) );
        }

        [Test]
        public void HasEnoughUnitsIsTrue_WhenEnoughUnits() {
            SetPlayerDataToEnoughUnits();

            Assert.IsTrue( mTestSelection.HasEnoughUnits() );
        }

        [Test]
        public void HasEnoughUnitsIsFalse_WhenNotEnoughUnits() {
            SetPlayerDataToNotEnoughUnits();

            Assert.IsFalse( mTestSelection.HasEnoughUnits() );
        }
        
        [Test]
        public void CorrectColorProperty_WhenEnoughUnits() {
            Color color = Constants.GetConstant<Color>( ConstantKeys.ENOUGH_UNITS_COLOR );
            SetPlayerDataToEnoughUnits();
            mTestSelection.RecalculateProperties();            

            Assert.AreEqual( color, mTestSelection.ViewModel.GetPropertyValue<Color>( MissionKeys.NUM_UNITS_FOR_TASK_COLOR ) );
        }

        [Test]
        public void CorrectColorProperty_WhenNotEnoughUnits() {
            Color color = Constants.GetConstant<Color>( ConstantKeys.NOT_ENOUGH_UNITS_COLOR );
            SetPlayerDataToNotEnoughUnits();
            mTestSelection.RecalculateProperties();

            Assert.AreEqual( color, mTestSelection.ViewModel.GetPropertyValue<Color>( MissionKeys.NUM_UNITS_FOR_TASK_COLOR ) );
        }

        [Test]
        public void PromisedUnitsChangesCorrectly_OnUnitSelectionTrue() {
            SetPlayerDataToEnoughUnits();
            int unitsRequired = StatCalculator.Instance.GetNumUnitsForRequirement( mUnit, TEST_STAT, POWER_REQUIREMENT );
            mTestSelection.UnitSelected( true );
            
            Assert.AreEqual( unitsRequired, mPromisedUnits[mUnit] );
        }

        [Test]
        public void PromisedUnitsChangesCorrectly_OnUnitSelectionFalse() {
            SetPlayerDataToEnoughUnits();
            int unitsRequired = StatCalculator.Instance.GetNumUnitsForRequirement( mUnit, TEST_STAT, POWER_REQUIREMENT );
            mPromisedUnits[mUnit] = unitsRequired;
            mTestSelection.UnitSelected( false );

            Assert.AreEqual( 0, mPromisedUnits[mUnit] );
        }

        [Test]
        public void UnitSelectedWithNotEnoughUnits_PromisesNoUnits() {
            SetPlayerDataToNotEnoughUnits();
            mTestSelection.UnitSelected( true );

            Assert.IsFalse( mPromisedUnits.ContainsKey( mUnit ) );
        }

        [Test]
        public void UnselectedUnitThatGetsUnselected_ReturnsNoUnits() {
            SetPlayerDataToNotEnoughUnits();
            mTestSelection.UnitSelected( false );

            Assert.IsFalse( mPromisedUnits.ContainsKey( mUnit ) );
        }

        private void SetPlayerDataToEnoughUnits() {
            IBuildingUtils utils = Substitute.For<IBuildingUtils>();
            utils.GetNumUnits( Arg.Any<IUnit>() ).Returns( int.MaxValue );

            BuildingUtilsManager.Utils = utils;
        }

        private void SetPlayerDataToNotEnoughUnits() {
            IBuildingUtils utils = Substitute.For<IBuildingUtils>();
            utils.GetNumUnits( Arg.Any<IUnit>() ).Returns( 0 );

            BuildingUtilsManager.Utils = utils;
        }
    }
}
