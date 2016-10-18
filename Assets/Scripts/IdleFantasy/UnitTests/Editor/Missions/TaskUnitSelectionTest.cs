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
        private IMissionProposal mMissionProposal;

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

            mMissionProposal = CreateMockMissionProposal();
            mUnit = new MockUnit( 100 );
            mTestSelection = new TaskUnitSelection( mUnit, 
                new MissionTaskData() { Index = TEST_TASK_INDEX, StatRequirement = TEST_STAT, PowerRequirement = POWER_REQUIREMENT }, 
                mMissionProposal );

            SetPlayerDataToNotEnoughUnits();
        }

        private IMissionProposal CreateMockMissionProposal() {
            IMissionProposal mock = Substitute.For<IMissionProposal>();
            mock.PromisedUnits.Returns( new Dictionary<string, int>() );
            mock.TaskProposals.Returns( new Dictionary<int, MissionTaskProposal>() );

            return mock;
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
        public void VerifyNumUnitsOwnedProperty_IsCorrect() {
            SetNumUnitsToAmount( 100 );
            mTestSelection.RecalculateProperties();

            Assert.AreEqual( 100, mTestSelection.ViewModel.GetPropertyValue<int>( MissionKeys.NUM_UNITS_OWNED ) );
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
        public void OnSelection_ChangesColor() {
            Color expectedColor = Constants.GetConstant<Color>( ConstantKeys.SELECTED_UNITS_COLOR );

            mTestSelection.UnitSelected( true );

            Assert.AreEqual( expectedColor, mTestSelection.ViewModel.GetPropertyValue<Color>( MissionKeys.NUM_UNITS_FOR_TASK_COLOR ) );
        }

        [Test]
        public void OnSelection_AddsProposal() {
            mTestSelection.UnitSelected( true );

            mMissionProposal.Received().AddProposal( Arg.Any<int>(), Arg.Any<MissionTaskProposal>() );
        }

        [Test]
        public void OnUnselect_RemovesProposal() {
            mTestSelection.UnitSelected( false );

            mMissionProposal.Received().RemoveProposal( Arg.Any<int>(), Arg.Any<MissionTaskProposal>() );
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

        private void SetNumUnitsToAmount( int i_amount ) {
            IBuildingUtils utils = Substitute.For<IBuildingUtils>();
            utils.GetNumUnits( Arg.Any<IUnit>() ).Returns( i_amount );

            BuildingUtilsManager.Utils = utils;
        }
    }
}
