using MyLibrary;

namespace IdleFantasy {
    public class MapView : GroupView {
        private ViewModel mViewModel;

        private Map mMap;

        public void Init( Map i_map ) {
            mViewModel = i_map.ViewModel;
            mMap = i_map;
            SetModel( mViewModel );

            //SubscribeToMessages();            
        }
    }
}
