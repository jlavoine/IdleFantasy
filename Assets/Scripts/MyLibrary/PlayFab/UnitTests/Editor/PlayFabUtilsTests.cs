using NUnit.Framework;

#pragma warning disable 0414

namespace MyLibrary.PlayFab.UnitTests {
    [TestFixture]
    public class PlayFabUtilsTests {

        [Test]
        public void TestStringCleaning_ReturnsCleanString() {
            string dirtyString = "";
            dirtyString += "\"[";
            dirtyString += "]\"";
            dirtyString += "\"{";
            dirtyString += "}\"";
            dirtyString += "\\\"";
      
            string cleanString = dirtyString.CleanStringForJsonDeserialization();

            Assert.AreEqual( "[]{}\"", cleanString );
        }
    }
}
