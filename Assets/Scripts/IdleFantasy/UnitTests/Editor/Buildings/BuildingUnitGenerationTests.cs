using NUnit.Framework;
using System;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests {
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

        [Test]
        [TestCaseSource( "UnitGenerationAmounts" )]
        public void TestUnitGeneration( int i_numUnits ) {
            Building testBuilding = BuildingUpgradeTests.GetMockBuilding();
            int expectedUnits = Math.Max( 0, testBuilding.NumUnits + i_numUnits );
            expectedUnits = Math.Min( expectedUnits, testBuilding.Capacity );

            testBuilding.AddUnitsFromProgress( i_numUnits );
            
            Assert.AreEqual( testBuilding.NumUnits, expectedUnits );
        }

        [Test]
        [TestCaseSource( "TimeSpans" )]
        public void UnitsGeneratedFromTick( TimeSpan i_timeSpan ) {
            Building testBuilding = BuildingUpgradeTests.GetMockBuilding();
            IUnit unit = new MockUnit( 1 );
            testBuilding.SetUnit( unit );
            int unitsBeforeTick = testBuilding.NumUnits;

            testBuilding.Tick( i_timeSpan );

            Assert.AreNotSame( unitsBeforeTick, testBuilding.NumUnits );
        }

        [Test]
        public void ZeroProgressAtMaxCapacity() {
            Building testBuilding = BuildingUpgradeTests.GetMockBuilding();
            IUnit unit = new MockUnit( .111f );
            testBuilding.SetUnit( unit );

            testBuilding.Tick( new TimeSpan( 1, 0, 0 ) );

            Assert.AreEqual( 0, testBuilding.NextUnitProgress );
        }
    }
}