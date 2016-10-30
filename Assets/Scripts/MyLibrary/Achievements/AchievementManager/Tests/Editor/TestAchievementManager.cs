using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;

namespace MyLibrary.Achievements {
    [TestFixture]
    public class TestAchievementManager {

        [Test]
        public void UnearnedAchievementsList_ContainsUnearnedAchievements() {
            List<IAchievement> previouslyEarnedAchievements = new List<IAchievement>();
            IAchievement earnedAchievement = GetMockAchievementWithEarnedState( true );
            previouslyEarnedAchievements.Add( earnedAchievement );

            List<IAchievement> listAchievements = new List<IAchievement>();
            listAchievements.Add( earnedAchievement );
            listAchievements.Add( GetMockAchievementWithEarnedState( false ) );

            AchievementManager manager = new AchievementManager( previouslyEarnedAchievements, listAchievements, Substitute.For<IInfoPopupManager>(), "n/a" );

            Assert.AreEqual( 1, manager.UnearnedAchievements.Count );
        }

        [Test]
        public void EarningAnAchievement_RemovesFromUnearnedList() {
            List<IAchievement> emptyPreviouslyEarnedAchievements = new List<IAchievement>();
            List<IAchievement> listAchievements = new List<IAchievement>();
            IAchievement mockAchievementToBeEarned = GetMockAchievementWithEarnedState( false );
            listAchievements.Add( mockAchievementToBeEarned );

            AchievementManager manager = new AchievementManager( emptyPreviouslyEarnedAchievements, listAchievements, Substitute.For<IInfoPopupManager>(), "n/a" );
            mockAchievementToBeEarned.IsEarned().Returns( true );
            manager.CheckForNewAchievements();

            Assert.AreEqual( 0, manager.UnearnedAchievements.Count );
        }

        [Test]
        public void EarningAnAchievement_QueuesPopup() {
            List<IAchievement> emptyPreviouslyEarnedAchievements = new List<IAchievement>();
            IInfoPopupManager mockPopupManager = Substitute.For<IInfoPopupManager>();
            List<IAchievement> listAchievements = new List<IAchievement>();
            IAchievement mockAchievementToBeEarned = GetMockAchievementWithEarnedState( false );
            listAchievements.Add( mockAchievementToBeEarned );

            AchievementManager manager = new AchievementManager( emptyPreviouslyEarnedAchievements, listAchievements, mockPopupManager, "n/a" );
            mockAchievementToBeEarned.IsEarned().Returns( true );
            manager.CheckForNewAchievements();

            mockPopupManager.Received().QueueInfoPopup( Arg.Any<string>(), Arg.Any<IViewModel>() );
        }

        private IAchievement GetMockAchievementWithEarnedState( bool i_earnedState ) {
            IAchievement mockAchievement = Substitute.For<IAchievement>();
            mockAchievement.IsEarned().Returns( i_earnedState );

            return mockAchievement;
        }
    }
}
