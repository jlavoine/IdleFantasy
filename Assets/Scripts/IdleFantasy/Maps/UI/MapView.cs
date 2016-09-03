using MyLibrary;
using UnityEngine;

namespace IdleFantasy {
    public class MapView : GroupView {
        public GameObject MapAreaPrefab;
        public GameObject MapAreaContent;

        private ViewModel mViewModel;

        private Map mMap;

        public void Init( Map i_map ) {
            mViewModel = i_map.ViewModel;
            mMap = i_map;
            SetModel( mViewModel );

            CreateMapAreas();

            //SubscribeToMessages();            
        }

        private void CreateMapAreas() {
            foreach ( MapAreaData areaData in mMap.Data.Areas ) {
                GameObject areaObject = gameObject.InstantiateUI( MapAreaPrefab, MapAreaContent );
                MapAreaView areaView = areaObject.GetComponent<MapAreaView>();
                areaView.Init( new MapArea( areaData ) );
            }
        }
    }
}
