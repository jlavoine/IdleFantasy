using NUnit.Framework;
using MyLibrary;
using NSubstitute;
using System.Collections.Generic;

#pragma warning disable 0414
namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class RepeatableQuestModelTests {
        [SetUp]
        public void BeforeTests() {
            //UnitTestUtils.ReplaceMessengerWithMock();
        }

        [TearDown]
        public void AfterTests() {

        }

        [Test]
        public void QuestMissionVisible_IfQuestAvailableAndNotDone() {
            IRepeatableQuestProgress progress = Substitute.For<IRepeatableQuestProgress>();
            progress.IsQuestAvailable().Returns( true );
            progress.IsDone().Returns( false );
            progress.GetMissionData().Returns( GetMockMission() );
            RepeatableQuestModel model = new RepeatableQuestModel( progress, Substitute.For<IAdManager>() );

            Assert.AreEqual( 1f, model.Mission.ViewModel.GetPropertyValue<float>( RepeatableQuestModel.MISSION_VISIBLE_PROPERTY ) );
        }

        [Test]
        public void QuestMissionNotVisible_IfQuestNotAvailable() {
            IRepeatableQuestProgress progress = Substitute.For<IRepeatableQuestProgress>();
            progress.IsQuestAvailable().Returns( false );
            progress.GetMissionData().Returns( GetMockMission() );
            RepeatableQuestModel model = new RepeatableQuestModel( progress, Substitute.For<IAdManager>() );

            Assert.AreEqual( 0f, model.Mission.ViewModel.GetPropertyValue<float>( RepeatableQuestModel.MISSION_VISIBLE_PROPERTY ) );
        }

        [Test]
        public void QuestMissionNotVisible_IfQuestDone() {
            IRepeatableQuestProgress progress = Substitute.For<IRepeatableQuestProgress>();
            progress.IsDone().Returns( true );
            progress.GetMissionData().Returns( GetMockMission() );
            RepeatableQuestModel model = new RepeatableQuestModel( progress, Substitute.For<IAdManager>() );

            Assert.AreEqual( 0f, model.Mission.ViewModel.GetPropertyValue<float>( RepeatableQuestModel.MISSION_VISIBLE_PROPERTY ) );
        }

        [Test]
        public void CountPropertyMatchesProgressCount() {
            int expectedCount = 111;
            IRepeatableQuestProgress progress = Substitute.For<IRepeatableQuestProgress>();
            progress.GetCompletedCount().Returns( expectedCount );
            progress.GetMissionData().Returns( GetMockMission() );
            RepeatableQuestModel model = new RepeatableQuestModel( progress, Substitute.For<IAdManager>() );

            Assert.AreEqual( expectedCount, model.ViewModel.GetPropertyValue<int>( RepeatableQuestModel.CURRENT_COMPLETED_COUNT_PROPERTY ) );
        }

        [Test]
        public void AdPanelVisible_IfQuestNotAvailable() {
            IRepeatableQuestProgress progress = Substitute.For<IRepeatableQuestProgress>();
            progress.IsQuestAvailable().Returns( false );
            progress.GetMissionData().Returns( GetMockMission() );
            RepeatableQuestModel model = new RepeatableQuestModel( progress, Substitute.For<IAdManager>() );

            Assert.AreEqual( 1f, model.ViewModel.GetPropertyValue<float>( RepeatableQuestModel.AD_VISIBLE_PROPERTY ) );
        }

        [Test]
        public void AdPanelVisible_IfQuestDone() {
            IRepeatableQuestProgress progress = Substitute.For<IRepeatableQuestProgress>();
            progress.IsDone().Returns( true );
            progress.GetMissionData().Returns( GetMockMission() );
            RepeatableQuestModel model = new RepeatableQuestModel( progress, Substitute.For<IAdManager>() );

            Assert.AreEqual( 1f, model.ViewModel.GetPropertyValue<float>( RepeatableQuestModel.AD_VISIBLE_PROPERTY ) );
        }

        [Test]
        public void AdPanelNotVisible_IfQuestIsAvailableAndNotDone() {
            IRepeatableQuestProgress progress = Substitute.For<IRepeatableQuestProgress>();
            progress.IsQuestAvailable().Returns( true );
            progress.IsDone().Returns( false );
            progress.GetMissionData().Returns( GetMockMission() );
            RepeatableQuestModel model = new RepeatableQuestModel( progress, Substitute.For<IAdManager>() );

            Assert.AreEqual( 0f, model.ViewModel.GetPropertyValue<float>( RepeatableQuestModel.AD_VISIBLE_PROPERTY ) );
        }

        [Test]
        public void AdPanelInteractble_IfAdIsReadyAndQuestNotAvailableAndNotDone() {
            IRepeatableQuestProgress progress = Substitute.For<IRepeatableQuestProgress>();
            progress.IsQuestAvailable().Returns( false );
            progress.IsDone().Returns( false );
            progress.GetMissionData().Returns( GetMockMission() );
            IAdManager ads = Substitute.For<IAdManager>();
            ads.IsAdReady().Returns( true );
            RepeatableQuestModel model = new RepeatableQuestModel( progress, ads );

            Assert.IsTrue( model.ViewModel.GetPropertyValue<bool>( RepeatableQuestModel.AD_INTERACTABLE_PROPERTY ) );
        }

        [Test]
        public void AdPanelNotInteractable_IfAdIsNotReadyButOtherwiseOk() {
            IRepeatableQuestProgress progress = Substitute.For<IRepeatableQuestProgress>();
            progress.IsQuestAvailable().Returns( false );
            progress.IsDone().Returns( false );
            progress.GetMissionData().Returns( GetMockMission() );
            IAdManager ads = Substitute.For<IAdManager>();
            ads.IsAdReady().Returns( false );
            RepeatableQuestModel model = new RepeatableQuestModel( progress, ads );

            Assert.IsFalse( model.ViewModel.GetPropertyValue<bool>( RepeatableQuestModel.AD_INTERACTABLE_PROPERTY ) );
        }

        [Test]
        public void AdPanelNotInteractable_IfQuestAvailableButOtherwiseOk() {
            IRepeatableQuestProgress progress = Substitute.For<IRepeatableQuestProgress>();
            progress.IsQuestAvailable().Returns( true );
            progress.IsDone().Returns( false );
            progress.GetMissionData().Returns( GetMockMission() );
            IAdManager ads = Substitute.For<IAdManager>();
            ads.IsAdReady().Returns( true );
            RepeatableQuestModel model = new RepeatableQuestModel( progress, ads );

            Assert.IsFalse( model.ViewModel.GetPropertyValue<bool>( RepeatableQuestModel.AD_INTERACTABLE_PROPERTY ) );
        }

        [Test]
        public void AdPanelNotInteratable_IfQuestDoneButOtherwiseOk() {
            IRepeatableQuestProgress progress = Substitute.For<IRepeatableQuestProgress>();
            progress.IsQuestAvailable().Returns( false );
            progress.IsDone().Returns( true );
            progress.GetMissionData().Returns( GetMockMission() );
            IAdManager ads = Substitute.For<IAdManager>();
            ads.IsAdReady().Returns( true );
            RepeatableQuestModel model = new RepeatableQuestModel( progress, ads );

            Assert.IsFalse( model.ViewModel.GetPropertyValue<bool>( RepeatableQuestModel.AD_INTERACTABLE_PROPERTY ) );
        }

        private MissionData GetMockMission() {
            // oops...this should've been an interface
            return new MissionData() { Tasks = new List<MissionTaskData>() };
        }
    }
}