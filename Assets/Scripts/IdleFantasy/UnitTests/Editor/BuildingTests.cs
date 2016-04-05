using System.Collections.Generic;
using NUnit.Framework;

namespace IdleFantasy.UnitTests {
    public class BuildingTests {

        static object[] UpgradeCases = {
            new object[] { 1, 2 },
            new object[] { 10, 11 },
            new object[] { 22, 23 },
            new object[] { -10, 1 },
            new object[] { 50, 50 }
        };

        [Test]
        [TestCaseSource("UpgradeCases")]
        public void UpgradeTest(int i_level, int i_expectedLevel) {
            Building testBuilding = GetMockBuilding();
            testBuilding.Level = i_level;

            testBuilding.Upgrade();

            Assert.AreEqual( testBuilding.Level, i_expectedLevel );
        }

        private Building GetMockBuilding() {
            BuildingData data = new BuildingData();
            data.ID = "TEST_BUILDING";
            data.MaxLevel = 50;
            data.Size = 10;
            data.Units = new List<string>() { "TEST_UNIT" };
            data.Categories = new List<string>() { "TEST_CATEGORY" };

            return new Building( data );
        }
    }
}