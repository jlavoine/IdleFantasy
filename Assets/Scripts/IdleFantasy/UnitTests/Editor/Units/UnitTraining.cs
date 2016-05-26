using NUnit.Framework;
using MyLibrary;
using System.Collections.Generic;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class UnitTraining {
        private ITrainerManager mTrainerData;
        private IUnit mUnit;

        static object[] UnitTrainingChanges = {
            new object[] { 1, true, 2 },
            new object[] { 1, false, 0 },
            new object[] { 0, true, 1 },
            new object[] { 0, false, 0 },
            new object[] { -1, true, 1 },
            new object[] { -1, false, 0 }
        };

        static object[] UnitTrainingWithCosts = {
            new object[] { 1, 2 },
            new object[] { 0, 1 },
            new object[] { -1, 1 },
            new object[] { 10, 11 },
            new object[] { 100, 101 }
        };

        static object[] CanTrainUnitsSource = {
            new object[] { 1, 2, true },
            new object[] { 0, 1, true },
            new object[] { 0, 0, false },
            new object[] { 5, 6, true }
        };

        static object[] CanUntrainUnitsSource = {
            new object[] { 1, true },
            new object[] { 0, false },
            new object[] { -1, false },
            new object[] { 5, true }
        };

        [SetUp]
        public void BeforeTests() {
            UnitTestUtils.LoadOfflineData();
            mTrainerData = new TrainerManager( new ViewModel(), new TrainerSaveData() );
            mUnit = new Unit( GenericDataLoader.GetData<UnitData>( GenericDataLoader.UNITS, GenericDataLoader.TEST_UNIT ), new ViewModel() );
        }

        [Test]
        [TestCaseSource("UnitTrainingChanges")]
        public void TrainingChangesResultInExpected( int i_trainingLevel, bool i_isTraining, int i_expectedLevel ) {
            mUnit.TrainingLevel = i_trainingLevel;

            mTrainerData.ChangeUnitTrainingLevel( mUnit, i_isTraining );

            Assert.AreEqual( i_expectedLevel, mUnit.TrainingLevel );
        }

        [Test]
        [TestCaseSource("UnitTrainingWithCosts")]
        public void VerifyUnitTrainingCosts( int i_trainingLevel, int i_expectedCostInTrainers ) {
            mUnit.TrainingLevel = i_trainingLevel;

            int costInTrainers = mTrainerData.GetTrainersToTrainUnit( mUnit );

            Assert.AreEqual( i_expectedCostInTrainers, costInTrainers );
        }

        [Test]
        public void TrainingUnitIncreasesTrainingLevel() {
            mUnit.TrainingLevel = 1;

            mTrainerData.ChangeUnitTrainingLevel( mUnit, true );

            Assert.AreEqual( 2, mUnit.TrainingLevel );
        }

        [Test]
        public void UntrainingUnitDecreasesTrainingLevel() {
            mUnit.TrainingLevel = 2;

            mTrainerData.ChangeUnitTrainingLevel( mUnit, false );

            Assert.AreEqual( 1, mUnit.TrainingLevel );
        }

        [Test]
        [TestCaseSource( "CanTrainUnitsSource")]
        public void CanIncreaseUnitTraining( int i_level, int i_availableTrainers, bool i_expectedResult ) {
            mUnit.TrainingLevel = i_level;
            mTrainerData.TotalTrainers = i_availableTrainers;
            mTrainerData.AvailableTrainers = i_availableTrainers;

            bool canTrain = mTrainerData.CanChangeUnitTraining( mUnit, true );

            Assert.AreEqual( canTrain, i_expectedResult );
        }

        [Test]
        [TestCaseSource( "CanUntrainUnitsSource" )]
        public void CanDecreaseUnitTraining( int i_level, bool i_expectedResult ) {
            mUnit.TrainingLevel = i_level;

            bool canTrain = mTrainerData.CanChangeUnitTraining( mUnit, false );

            Assert.AreEqual( canTrain, i_expectedResult );
        }

        [Test]
        public void VerifyInitiateUnitTraining() {
            mTrainerData.TotalTrainers = 1;
            mTrainerData.AvailableTrainers = 1;
            mUnit.TrainingLevel = 0;

            mTrainerData.InitiateChangeInTraining( mUnit, true );

            Assert.AreEqual( 1, mUnit.TrainingLevel );
        }

        [Test]
        public void VerifyInitiateUnitUntraining() {
            mUnit.TrainingLevel = 1;

            mTrainerData.InitiateChangeInTraining( mUnit, false );

            Assert.AreEqual( 0, mUnit.TrainingLevel );
        }
    }
}