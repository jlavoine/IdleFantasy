using NUnit.Framework;
using NSubstitute;

namespace MyLibrary.Achievements {
    [TestFixture]
    public class TestAchievementRequirement {
        
        [Test]
        public void RequirementPasses_WhenGameMetricMeetRequiredCount() {
            IGameMetrics mockMetrics = Substitute.For<IGameMetrics>();
            mockMetrics.GetMetric( Arg.Any<string>() ).Returns( int.MaxValue );

            AchievementRequirement req = new AchievementRequirement( "TestRequirement", 100, mockMetrics );

            Assert.IsTrue( req.DoesPass() );
        }

        [Test]
        public void RequirementDoesNotPass_WhenGameMetricDoesNotMeetRequiredCount() {
            IGameMetrics mockMetrics = Substitute.For<IGameMetrics>();
            mockMetrics.GetMetric( Arg.Any<string>() ).Returns( 0 );

            AchievementRequirement req = new AchievementRequirement( "TestRequirement", 100, mockMetrics );

            Assert.IsFalse( req.DoesPass() );
        }
    }
}
