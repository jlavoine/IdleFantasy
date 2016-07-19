using NUnit.Framework;
using System.Collections.Generic;

#pragma warning disable 0414
namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class MissionProposalTest {

        private MissionTaskProposal mTestTaskProposal;

        [SetUp]
        public void BeforeTests() {
            mTestTaskProposal = new MissionTaskProposal( 0, "Test", 100 );
        }

        [Test]
        public void AddProposal_AddsProposal() {
            MissionProposal missionProposal = new MissionProposal();

            missionProposal.AddProposal( mTestTaskProposal.TaskIndex, mTestTaskProposal );

            Assert.AreEqual( missionProposal.TaskProposals[mTestTaskProposal.TaskIndex], mTestTaskProposal );
            Assert.AreEqual( missionProposal.PromisedUnits[mTestTaskProposal.UnitID], mTestTaskProposal.UnitCount );
        }

        [Test]
        public void RemoveProposal_RemovesProposal() {
            Dictionary<int, MissionTaskProposal> taskProposals = new Dictionary<int, MissionTaskProposal>() { { mTestTaskProposal.TaskIndex, mTestTaskProposal } };
            Dictionary<string, int> promisedUnits = new Dictionary<string, int>() { { mTestTaskProposal.UnitID, mTestTaskProposal.UnitCount } };
            MissionProposal missionProposal = new MissionProposal( promisedUnits, taskProposals );

            missionProposal.RemoveProposal( mTestTaskProposal.TaskIndex, mTestTaskProposal );

            Assert.AreEqual( missionProposal.TaskProposals[mTestTaskProposal.TaskIndex], null );
            Assert.AreEqual( missionProposal.PromisedUnits[mTestTaskProposal.UnitID], 0 );
        }
    }
}
