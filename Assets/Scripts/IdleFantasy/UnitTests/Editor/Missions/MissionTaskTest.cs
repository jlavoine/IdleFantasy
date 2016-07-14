using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;
using IdleFantasy.UnitTests.Units;
using NSubstitute;

#pragma warning disable 0414
namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class MissionTaskTest {

        private IPlayerData mPlayerData;
        private MissionTask mMissionTask;

        private IUnit mTestUnit = new MockUnit( 100 );

        private const string DESCRIPTION = "This is a description.";
        private const string TEST_STAT = TestUnitStats.TEST_STAT_1;
        private const int REQUIRED_POWER = 1000;


        [SetUp]
        public void BeforeTest() {
            UnitTestUtils.LoadOfflineData();
            mPlayerData = UnitTestUtils.LoadMockPlayerData();

            SetStatCalculator();            
            CreateMissionTask();
        }

        [TearDown]
        public void AfterTest() {
            StatCalculator.Instance = null;
        }

        private void SetStatCalculator() {
            List<IUnit> testList = new List<IUnit>();
            testList.Add( mTestUnit );
            
            IStatCalculator calculator = Substitute.For<IStatCalculator>();
            calculator.GetUnitsWithStat( Arg.Any<string>() ).Returns( testList );

            StatCalculator.Instance = calculator;
        }

        private void CreateMissionTask() {
            MissionTaskData data = new MissionTaskData();
            data.DescriptionKey = DESCRIPTION;
            data.PowerRequirement = REQUIRED_POWER;
            data.StatRequirement = TEST_STAT;

            mMissionTask = new MissionTask( data, new Dictionary<IUnit, int>() );
        }

        [Test]
        public void VerifyPropertes_FromMissionTaskCreated() {
            Assert.AreEqual( DESCRIPTION, mMissionTask.ViewModel.GetPropertyValue<string>( MissionKeys.DESCRIPTION ) );
            Assert.AreEqual( TEST_STAT, mMissionTask.ViewModel.GetPropertyValue<string>( MissionKeys.TASK_STAT ) );
            Assert.AreEqual( REQUIRED_POWER, mMissionTask.ViewModel.GetPropertyValue<int>( MissionKeys.TASK_POWER ) );
        }

        [Test]
        public void CorrectUnitsEligible_ForMissionTask() {
            Assert.AreEqual( 1, mMissionTask.UnitsEligibleForTask.Count );
            Assert.AreEqual( mTestUnit.GetID(), mMissionTask.UnitsEligibleForTask[0].Unit.GetID() );
        }
    }
}
