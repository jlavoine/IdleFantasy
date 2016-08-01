using NUnit.Framework;

namespace MyLibrary.UnitTests {
    [TestFixture]
    public class NumberRestrictionTests {

        private const float MIN_VALUE = 0f;
        private const float MID_VALUE = 50f;
        private const float MAX_VALUE = 100f;

        private NumberRestriction mRestrictionUnderTest;

        [SetUp]
        public void BeforeTest() {
            mRestrictionUnderTest = new NumberRestriction();
            mRestrictionUnderTest.Min = MIN_VALUE;
            mRestrictionUnderTest.Max = MAX_VALUE;
        }

        [Test]
        public void ValueInBetweenMinAndMax_Passes() {
            Assert.IsTrue( mRestrictionUnderTest.DoesPass( MID_VALUE ) );
        }

        [Test]
        public void ValueLessThanMin_Fails() {
            Assert.IsFalse( mRestrictionUnderTest.DoesPass( MIN_VALUE-1 ) );
        }

        [Test]
        public void ValueGreaterThanMax_Fails() {
            Assert.IsFalse( mRestrictionUnderTest.DoesPass( MAX_VALUE + 1 ) );
        }

        [Test]
        public void ValueEqualToMin_Passes() {
            Assert.IsTrue( mRestrictionUnderTest.DoesPass( MIN_VALUE ) );
        }

        [Test]
        public void ValueEqualToMax_Passes() {
            Assert.IsTrue( mRestrictionUnderTest.DoesPass( MAX_VALUE ) );
        }
    }
}
