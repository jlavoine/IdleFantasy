using NUnit.Framework;
using MyLibrary;
using System.Collections.Generic;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class TrainingTests {
        private ITrainerData mTrainerData;

        static object[] NewTrainerCostTests = {
            new object[] { 0, 1000 },
            new object[] { -1, 1000 },
            new object[] { 10, 11000 },
            new object[] { 1, 2000 },
            new object[] { 20, 21000 }
        };

        [SetUp]
        public void BeforeTests() {
            //Dictionary<string, int> trainers = new Dictionary<string, int>() { { TrainerData.NORMAL_TRAINERS } };

            mTrainerData = new TrainerData( new ViewModel(), new Dictionary<string, int>() );
        }

        [Test]
        public void AddTrainerAdds() {
            mTrainerData.TotalTrainers = 1;
            mTrainerData.AvailableTrainers = 1;

            mTrainerData.AddTrainer( TrainerData.NORMAL_TRAINERS, 1 );

            Assert.AreEqual( 2, mTrainerData.AvailableTrainers );
        }

        [Test]
        public void TotalTrainerCalculatedFromAllTrainerTypes() {
            Dictionary<string, int> trainers = new Dictionary<string, int>();
            trainers.Add( "Type_1", 1 );
            trainers.Add( "Type_2", 3 );
            trainers.Add( "Type_3", 5 );

            mTrainerData = new TrainerData( new ViewModel(), trainers );

            Assert.AreEqual( mTrainerData.TotalTrainers, 9 );
        }

        [Test]
        public void CannotAffordNewTrainerWithEmptyInventory() {
            bool canAfford = mTrainerData.CanAffordTrainerPurchase( new EmptyInventory() );

            Assert.IsFalse( canAfford );
        }

        [Test]
        public void CanAffordNewTrainerWithFullInventory() {
            bool canAfford = mTrainerData.CanAffordTrainerPurchase( new FullInventory() );

            Assert.IsTrue( canAfford );
        }

        [Test]
        [TestCaseSource( "NewTrainerCostTests" )]
        public void VerifyNextTrainerCosts( int i_numTrainers, int i_expectedCostForNextTrainer ) {
            Dictionary<string, int> trainers = new Dictionary<string, int>();
            trainers.Add( TrainerData.NORMAL_TRAINERS, i_numTrainers );

            mTrainerData = new TrainerData( new ViewModel(), trainers );

            int costForNextTrainer = mTrainerData.GetNextTrainerCost();

            Assert.AreEqual( i_expectedCostForNextTrainer, costForNextTrainer );
        }

        public void VerifyCanAffordNextTrain_RealInventory() {

        }

        // verify initiate next trainer path

        // verify unit training cost (in trainers)

        // verify training unit

        // verify untraining unit

        // verify train unit path

        // verify can train and untrain unit
    }
}
