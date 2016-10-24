using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;

namespace MyLibrary.Achievements {
    [TestFixture]
    public class TestAchievement {

        [Test]
        public void AchievementIsEarned_WhenAllRequirementsPass() {
            List<IAchievementRequirement> requirements = new List<IAchievementRequirement>();
            requirements.Add( CreateMockRequirementThatPasses() );
            requirements.Add( CreateMockRequirementThatPasses() );
            requirements.Add( CreateMockRequirementThatPasses() );

            Achievement achievement = new Achievement( "TestAchievement", requirements );

            Assert.IsTrue( achievement.IsEarned() );
        }

        [Test]
        public void AchievementIsNotEarned_WhenAnyRequirementDoesNotPass() {
            List<IAchievementRequirement> requirements = new List<IAchievementRequirement>();
            requirements.Add( CreateMockRequirementThatPasses() );
            requirements.Add( CreateMockRequirementThatPasses() );
            requirements.Add( CreateMockRequirementThatDoesNotPass() );

            Achievement achievement = new Achievement( "TestAchievement", requirements );

            Assert.IsFalse( achievement.IsEarned() );
        }

        private IAchievementRequirement CreateMockRequirementThatPasses() {
            IAchievementRequirement req = Substitute.For<IAchievementRequirement>();
            req.DoesPass().Returns( true );

            return req;
        }

        private IAchievementRequirement CreateMockRequirementThatDoesNotPass() {
            IAchievementRequirement req = Substitute.For<IAchievementRequirement>();
            req.DoesPass().Returns( false );

            return req;
        }
    }
}
