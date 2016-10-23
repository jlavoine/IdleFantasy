using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class TestUnitInfoPM {
        private const string UNIT_1_ID = "Unit1Id";
        private const string UNIT_1_NAME = "Unit1Name";

        [Test]
        public void UnitInfoPM_SetsCorrectUnit_FromIndex() {
            List<IUnit> mockUnits = GetUnitsList();
            UnitInfoPM testPM = new UnitInfoPM( mockUnits, 1, Substitute.For<IStatCalculator>() );

            Assert.AreEqual( mockUnits[1], testPM.SelectedUnit );
        }

        [Test]
        public void UnitInfoPM_GoingToNextUnit_GoesToNextUnit() {
            List<IUnit> mockUnits = GetUnitsList();
            UnitInfoPM testPM = new UnitInfoPM( mockUnits, 1, Substitute.For<IStatCalculator>() );

            testPM.GoToNextUnit();

            Assert.AreEqual( mockUnits[2], testPM.SelectedUnit );
        }

        [Test]
        public void UnitInfoPM_GoingToPreviousUnit_GoesToPreviousUnit() {
            List<IUnit> mockUnits = GetUnitsList();
            UnitInfoPM testPM = new UnitInfoPM( mockUnits, 1, Substitute.For<IStatCalculator>() );

            testPM.GoToPreviousUnit();

            Assert.AreEqual( mockUnits[0], testPM.SelectedUnit );
        }

        [Test]
        public void UnitInfoPM_GoingToPreviousUnitAtFirstUnit_WrapsToLastUnit() {
            List<IUnit> mockUnits = GetUnitsList();
            UnitInfoPM testPM = new UnitInfoPM( mockUnits, 0, Substitute.For<IStatCalculator>() );

            testPM.GoToPreviousUnit();

            Assert.AreEqual( mockUnits[2], testPM.SelectedUnit );
        }

        [Test]
        public void UnitInfoPM_GoingToNextUnitAtLastUnit_WrapsToFirstUnit() {
            List<IUnit> mockUnits = GetUnitsList();
            UnitInfoPM testPM = new UnitInfoPM( mockUnits, 2, Substitute.For<IStatCalculator>() );

            testPM.GoToNextUnit();

            Assert.AreEqual( mockUnits[0], testPM.SelectedUnit );
        }

        [Test]
        public void UnitInfoPM_IdProperty_MatchesUnit() {
            List<IUnit> mockUnits = GetUnitsList();
            UnitInfoPM testPM = new UnitInfoPM( mockUnits, 0, Substitute.For<IStatCalculator>() );

            Assert.AreEqual( UNIT_1_ID, testPM.ViewModel.GetPropertyValue<string>( UnitInfoPM.UNIT_ID_PROPERTY ) );
        }

        [Test]
        public void UnitInfoPM_NameProperty_MatchesUnit() {
            List<IUnit> mockUnits = GetUnitsList();
            UnitInfoPM testPM = new UnitInfoPM( mockUnits, 0, Substitute.For<IStatCalculator>() );

            Assert.AreEqual( UNIT_1_NAME, testPM.ViewModel.GetPropertyValue<string>( UnitInfoPM.UNIT_NAME_PROPERTY ) );
        }

        private List<IUnit> GetUnitsList() {
            List<IUnit> mockList = new List<IUnit>();

            IUnit mockUnit_1 = Substitute.For<IUnit>();
            mockUnit_1.GetName().Returns( UNIT_1_NAME );
            mockUnit_1.GetID().Returns( UNIT_1_ID );
            mockUnit_1.GetStats().Returns( new List<string>() );
            mockList.Add( mockUnit_1 );

            IUnit mockUnit_2 = Substitute.For<IUnit>();
            mockUnit_2.GetStats().Returns( new List<string>() );
            mockList.Add( mockUnit_2 );

            IUnit mockUnit_3 = Substitute.For<IUnit>();
            mockUnit_3.GetStats().Returns( new List<string>() );
            mockList.Add( mockUnit_3 );

            return mockList;
        }
    }
}