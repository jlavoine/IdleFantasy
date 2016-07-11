using NUnit.Framework;
using NSubstitute;

namespace MyLibrary.UnitTests {
    public class QueuedInfoPopupManagerTests {

        private InfoPopupManager mManager;

        [SetUp]
        public void BeforeTests() {
            mManager = Substitute.ForPartsOf<InfoPopupManager>();
            mManager.When( x => x.CreatePopup( Arg.Any<QueuedInfoPopupData>() ) ).DoNotCallBase();
        }

        [TearDown]
        public void AfterTests() {
            mManager.Dispose();
        }

        [Test]
        public void NewPopupManager_ShowingNoPopups() {
            Assert.AreEqual( false, mManager.ShowingPopup );
            Assert.IsEmpty( mManager.PendingPopups );
        }

        [Test]
        public void QueuePopup_ShowsPopup() {
            mManager.QueueInfoPopup( "", new ViewModel() );            

            Assert.IsTrue( mManager.ShowingPopup );
        }

        [Test]
        public void QueueMultiplePopups_AddsSecondToList() {            
            mManager.QueueInfoPopup( "one", new ViewModel() );
            mManager.QueueInfoPopup( "two", new ViewModel() );

            Assert.AreEqual( "two", mManager.PendingPopups[0].PrefabName );
        }

        [Test]
        public void QueuePopup_RemovesPopupFromPendingList() {
            mManager.QueueInfoPopup( "", new ViewModel() );
            Assert.IsEmpty( mManager.PendingPopups );
        }
        
        [Test]
        public void ClosingPopup_ShowsPendingPopup() {
            mManager.QueueInfoPopup( "one", new ViewModel() );
            mManager.QueueInfoPopup( "two", new ViewModel() );

            mManager.OnPopupClosed();

            Assert.IsEmpty( mManager.PendingPopups );
        }
        
        [Test]
        public void NoPopupShowing_AfterPopupClosed() {
            mManager.QueueInfoPopup( "one", new ViewModel() );

            mManager.OnPopupClosed();

            Assert.IsFalse( mManager.ShowingPopup );
        }        
    }
}