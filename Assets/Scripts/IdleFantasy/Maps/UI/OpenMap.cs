using UnityEngine;

namespace IdleFantasy {
    public class OpenMap : MonoBehaviour {
        public GameObject MapPrefab;
        public GameObject MainCanvas;

        private Map mMap;

        public void OnClick() {
            CreateMapView();
        }

        private void CreateMapView() {
            mMap = new Map( PlayerManager.Data.Maps[BackendConstants.WORLD_BASE] );
            GameObject mapUI = gameObject.InstantiateUI( MapPrefab, MainCanvas );
            MapView view = mapUI.GetComponent<MapView>();
            view.Init( mMap );
        }
    }
}
