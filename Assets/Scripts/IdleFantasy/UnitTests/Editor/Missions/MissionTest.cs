using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using MyLibrary;
using IdleFantasy.UnitTests.Units;
using NSubstitute;

#pragma warning disable 0414
namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class MissionTest {

        private IPlayerData mPlayerData;
        private Mission mMission;
        private MissionTaskData mTestTaskData;

        private const string MISSION_DESCRIPTION = "Mission Description";

        [SetUp]
        public void BeforeTest() {
            UnitTestUtils.LoadOfflineData();
            mPlayerData = UnitTestUtils.LoadMockPlayerData();

            CreateMission();
        }

        private void CreateMission() {
            MissionData data = new MissionData();
            data.DescriptionKey = MISSION_DESCRIPTION;
            data.Tasks = GetTestMissionTaskList();

            mMission = new Mission( data );
        }

        private List<MissionTaskData> GetTestMissionTaskList() {
            mTestTaskData = new MissionTaskData();
            mTestTaskData.PowerRequirement = 0;
            mTestTaskData.StatRequirement = "";
            mTestTaskData.DescriptionKey = "";
            List<MissionTaskData> taskDatas = new List<MissionTaskData>() { mTestTaskData };

            return taskDatas;
        }

        [Test]
        public void VerifyProperties_FromMissionCreated() {
            Assert.AreEqual( MISSION_DESCRIPTION, mMission.ViewModel.GetPropertyValue<string>( MissionKeys.DESCRIPTION ) );
        }

        [Test]
        public void VerifyTasksList_FromMissionCreated() {
            Assert.AreEqual( 1, mMission.Tasks.Count );
            Assert.AreEqual( mTestTaskData, mMission.Tasks[0].Data );
        }

        [Test]
        public void CompletingMission_CallsAlterUnits() {
            IBuildingUtils utils = Substitute.For<IBuildingUtils>();
            BuildingUtilsManager.Utils = utils;
            mMission.PromisedUnits = new Dictionary<IUnit, int>() { { new MockUnit( 100 ), 100 } };
            mMission.CompleteMission();

            utils.Received().AlterUnitCount( Arg.Any<IUnit>(), Arg.Any<int>() );

        }
    }
}
