using NUnit.Framework;
using MyLibrary;
using System.Collections.Generic;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class TestTrainerWorldReset {
        private ITrainerManager mTrainerData;

        [SetUp]
        public void BeforeTest() {
            UnitTestUtils.LoadOfflineData();
            UnitTestUtils.LoadMockPlayerData();

            CreateTrainerManager();
        }

        private void CreateTrainerManager() {
            TrainerSaveData saveData = new TrainerSaveData();
            saveData.TrainerCounts = new Dictionary<string, int>();
            saveData.TrainerCounts[TrainerManager.NORMAL_TRAINERS] = 1;

            mTrainerData = new TrainerManager( new ViewModel(), saveData, new Dictionary<string, UnitProgress>() );
        }

        [TearDown]
        public void AfterTests() {
            mTrainerData.Dispose();
            EasyMessenger.Instance = null;
        }

        [Test]
        public void NormalTrainersReset_AfterWorldReset() {
            EasyMessenger.Instance.Send( MapKeys.WORLD_RESET_SUCCESS );

            Assert.AreEqual( 0, mTrainerData.GetTotalTrainersOfType( TrainerManager.NORMAL_TRAINERS ) );
        }
    }
}
