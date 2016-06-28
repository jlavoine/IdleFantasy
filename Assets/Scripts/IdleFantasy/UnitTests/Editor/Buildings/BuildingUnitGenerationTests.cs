using NUnit.Framework;
using System;
using MyLibrary;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests {
    [TestFixture]
    public class BuildingUnitGenerationTests {
        static object[] UnitGenerationAmounts = {
            new object[] { 1 },
            new object[] { 0 },
            new object[] { 10 },
            new object[] { 100 },
            new object[] { -10 },
            new object[] { 50 }
        };

        static object[] TimeSpans = {
            new object[] { new TimeSpan(0, 0, 10) },
            new object[] { new TimeSpan(0, 0, 0) },
            new object[] { new TimeSpan(10, 0, 10) },
            new object[] { new TimeSpan(1, 0, 0) },
            new object[] { new TimeSpan(0, 2, 0) },
            new object[] { new TimeSpan(100, 10, 10) },
        };

        [SetUp]
        public void BeforeTest() {
            UnitTestUtils.LoadOfflineData();
        }

        [Test]
        [TestCaseSource( "UnitGenerationAmounts" )]
        public void TestUnitGeneration( int i_numUnits ) {
            Building testBuilding = BuildingUpgradeTests.GetTestBuilding();
            int expectedUnits = Math.Max( 0, testBuilding.NumUnits + i_numUnits );
            expectedUnits = Math.Min( expectedUnits, testBuilding.Capacity );

            testBuilding.AddUnitsFromProgress( i_numUnits );
            
            Assert.AreEqual( testBuilding.NumUnits, expectedUnits );
        }

        [Test]
        [TestCaseSource( "TimeSpans" )]
        public void UnitsGeneratedFromTick( TimeSpan i_timeSpan ) {
            Building testBuilding = BuildingUpgradeTests.GetTestBuilding();
            IUnit unit = new MockUnit( 1 );
            testBuilding.Unit = unit;
            int unitsBeforeTick = testBuilding.NumUnits;

            testBuilding.Tick( i_timeSpan );

            Assert.AreNotSame( unitsBeforeTick, testBuilding.NumUnits );
        }

        [Test]
        public void UpgradingUnitResetsCount() {
            Building testBuilding = BuildingUpgradeTests.GetTestBuilding();
            IUnit unit = new Unit( GenericDataLoader.GetData<UnitData>( GenericDataLoader.TEST_UNIT ),
                new UnitProgress() { Level = 1, Trainers = 1 },
                new ViewModel() );  // using a real Unit here is not great...should really use NSubstitute or something? But this was complex
            testBuilding.Unit = unit;
            testBuilding.Tick( new TimeSpan( 1, 0, 0 ) );
            
            unit.Level.Upgrade();

            Assert.AreEqual( 0, testBuilding.NumUnits );
        }

        [Test]
        public void ZeroProgressAtMaxCapacity() {
            Building testBuilding = BuildingUpgradeTests.GetTestBuilding();
            IUnit unit = new MockUnit( .111f );
            testBuilding.Unit = unit;

            testBuilding.Tick( new TimeSpan( 1, 0, 0 ) );

            Assert.AreEqual( 0, testBuilding.NextUnitProgress );
        }
    }
}