using UnityEngine;
using MyLibrary;

namespace IdleFantasy {
    public class OpenMap : MonoBehaviour {
        public GameObject MapPrefab;
        public GameObject MainCanvas;

        private Map mMap;

        public void Start() {
            SubscribeToMessages();
        }

        public void OnDestroy() {
            UnsubscribeFromMessages();
        }

        private void SubscribeToMessages() {
            MyMessenger.AddListener( MapKeys.TRAVEL_TO_SUCCESS, OnTravelSuccess );
        }

        private void UnsubscribeFromMessages() {
            MyMessenger.RemoveListener( MapKeys.TRAVEL_TO_SUCCESS, OnTravelSuccess );
        }

        private void OnTravelSuccess() {
            CreateMapView();
        }

        public void OnClick() {
            CreateMapView();
        }

        private void CreateMapView() {
            mMap = new Map( PlayerManager.Data.Maps[BackendConstants.WORLD_BASE] );

            GameObject mapUI = gameObject.InstantiateUI( MapPrefab, MainCanvas );
            MapView view = mapUI.GetComponent<MapView>();
            view.Init( mMap, PlayerManager.Data.MissionProgress[BackendConstants.WORLD_BASE] );
        }
    }
}
