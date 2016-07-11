using NUnit.Framework;

namespace MyLibrary.UnitTests {
    public class QueuedInfoPopupDataTests {

        [Test]
        public void QueuedInfoPopupData_ConstructorTest() {
            string popupPrefabName = "Test Popup";
            ViewModel popupViewModel = new ViewModel();
            QueuedInfoPopupData data = new QueuedInfoPopupData( popupPrefabName, popupViewModel );

            Assert.AreEqual( popupPrefabName, data.PrefabName );
            Assert.AreEqual( popupViewModel, data.ViewModel );
        }
    }
}
