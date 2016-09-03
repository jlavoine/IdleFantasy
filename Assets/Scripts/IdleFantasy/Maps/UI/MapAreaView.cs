using MyLibrary;

namespace IdleFantasy {
    public class MapAreaView : GroupView {
        #region Inspector

        #endregion

        private MapArea mArea;

        public void Init( MapArea i_area ) {
            mArea = i_area;

            SetModel( i_area.ViewModel );
        }

        public void OnMapAreaClicked() {
            MissionInterfaceManager.Instance.CreateUI( new Mission( mArea.Data.Mission ) );
        }
    }
}
