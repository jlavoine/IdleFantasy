using NUnit.Framework;
using NSubstitute;

namespace MyLibrary.Achievements {
    [TestFixture]
    public class TestEarnedAchievementPM {
        
        [Test]
        public void EarnedAchievementMainText_MatchesAchievementName() {
            string name = "Test";
            IAchievement mockAchievement = Substitute.For<IAchievement>();
            mockAchievement.GetName().Returns( name );

            EarnedAchievementPM pm = new EarnedAchievementPM( mockAchievement );

            Assert.AreEqual( name, pm.ViewModel.GetPropertyValue<string>( InfoPopupProperties.MAIN_TEXT ) );
        }
    }
}
