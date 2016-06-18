using System.Collections.Generic;
using NUnit.Framework;
using MyLibrary;

#pragma warning disable 0414

namespace IdleFantasy.UnitTests.Upgradeables {
    [TestFixture]
    public class UpgradeProgressTests {

        private PointsUpgradeable mUpgradeable;
        private ViewModel mTestModel;

        [SetUp]
        public void BeforeTest() {
            UnitTestUtils.LoadOfflineData();
            mTestModel = new ViewModel();

            UpgradeData testUpgradeData = new UpgradeData();
            testUpgradeData.MaxLevel = 50;
            testUpgradeData.PropertyName = "Level";
            testUpgradeData.BaseXpToLevel = 1000;

            mUpgradeable = new PointsUpgradeable();
            mUpgradeable.SetPropertyToUpgrade( mTestModel, testUpgradeData );
            mUpgradeable.Value = 1;
        }

        static object[] ProgressForLevel = {
            new object[] { 1, .5f, 1 },
            new object[] { 1, 1f, 2 },
            new object[] { 2, 1f, 3 },
            new object[] { 1, 2f, 3 },
            new object[] { 1, 1.5f, 2 }
        };

        [Test]
        [TestCaseSource("ProgressForLevel")]
        public void UpgradingByProgress_IncreasesLevel( int i_level, float i_progress, int i_expectedLevel ) {           
            mUpgradeable.Value = i_level;
            mUpgradeable.Points = 0;

            mUpgradeable.AddProgress( i_progress );

            Assert.AreEqual( i_expectedLevel, mUpgradeable.Value );
        }

        static object[] ProgressForXp = {
            new object[] { 1, .5f, 500 },
            new object[] { 1, 1f, 0 },
            new object[] { 2, 1f, 0 },
            new object[] { 1, 2f, 0 },
            new object[] { 1, 1.5f, 1000 }
        };

        [Test]
        [TestCaseSource( "ProgressForXp" )]
        public void UpgradingByProgress_SetsCorrectXp( int i_level, float i_progress, int i_expectedXp ) {
            mUpgradeable.Value = i_level;
            mUpgradeable.Points = 0;

            mUpgradeable.AddProgress( i_progress );

            Assert.AreEqual( i_expectedXp, mUpgradeable.Points );
        }

        static object[] PointsForLevel = {
            new object[] { 1, 1000, 2 },
            new object[] { 1, 500, 1 },
            new object[] { 2, 2000, 3 },
            new object[] { 1, 3000, 3 }
        };

        [Test]
        [TestCaseSource("PointsForLevel")]
        public void AddingPoints_LeadsToCorrectLevel( int i_level, int i_xp, int i_expectedLevel ) {
            mUpgradeable.Value = i_level;

            mUpgradeable.Points += i_xp;

            Assert.AreEqual( i_expectedLevel, mUpgradeable.Value );
        }

        [Test]
        public void PointsBelowZero_DoesNotWork() {            
            mUpgradeable.Points -= 1000;

            Assert.AreEqual( 0, mUpgradeable.Points );
        }
    }
}