using NUnit.Framework;

#pragma warning disable 0414

namespace MyLibrary.UnitTests {
    [TestFixture]
    public class StringRestrictionTests {

        private const string BASE_VALUE = "TestVal";
        private const string NOT_BASE_VALUE = "NotTheSameValAsBase";

        private StringRestriction mRestrictionUnderTest;

        [SetUp]
        public void BeforeTest() {
            mRestrictionUnderTest = new StringRestriction();
            mRestrictionUnderTest.BaseValue = BASE_VALUE;
        }

        static object[] TestCases = {
            new object[] { StringComparators.Ignore, string.Empty, true },
            new object[] { StringComparators.Ignore, BASE_VALUE, true },
            new object[] { StringComparators.Ignore, NOT_BASE_VALUE, true },
            new object[] { StringComparators.Match, string.Empty, false },
            new object[] { StringComparators.Match, BASE_VALUE, true },
            new object[] { StringComparators.Match, NOT_BASE_VALUE, false },
            new object[] { StringComparators.Mismatch, string.Empty, true },
            new object[] { StringComparators.Mismatch, NOT_BASE_VALUE, true },
            new object[] { StringComparators.Mismatch, BASE_VALUE, false }
        };

        [Test]
        [TestCaseSource("TestCases")]
        public void TestRestriction( StringComparators i_comparatorType, string i_value, bool i_expectedResult ) {
            mRestrictionUnderTest.Comparator = i_comparatorType;

            Assert.AreEqual( i_expectedResult, mRestrictionUnderTest.DoesPass( i_value ) );
        }
    }
}
