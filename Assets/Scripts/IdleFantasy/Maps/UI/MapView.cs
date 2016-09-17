using MyLibrary;
using UnityEngine;
using System.Collections.Generic;

namespace IdleFantasy {
    public class MapView : GroupView {
        public GameObject MapAreaPrefab;
        public GameObject MapAreaContent;

        private ViewModel mViewModel;

        private Map mMap;

        public void Init( Map i_map, WorldMissionProgress i_missionProgress ) {
            mViewModel = i_map.ViewModel;
            mMap = i_map;
            SetModel( mViewModel );

            CreateMapAreas( i_missionProgress.Missions );

            SubscribeToMessages();            
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            UnsubscribeFromMessages();
        }

        private void SubscribeToMessages() {
            MyMessenger.AddListener<int>( MapKeys.TRAVEL_TO_REQUEST, OnTravelToSelected );
        }

        private void UnsubscribeFromMessages() {
            MyMessenger.RemoveListener<int>( MapKeys.TRAVEL_TO_REQUEST, OnTravelToSelected );
        }

        private void CreateMapAreas( List<SingleMissionProgress> i_missionProgress ) {
            foreach ( MapAreaData areaData in mMap.Data.Areas ) {
                GameObject areaObject = gameObject.InstantiateUI( MapAreaPrefab, MapAreaContent );
                MapAreaView areaView = areaObject.GetComponent<MapAreaView>();
                areaView.Init( new MapArea( areaData, i_missionProgress[areaData.Index] ) );
            }
        }

        private void OnTravelToSelected( int i_travelOptionIndex ) {
            CloseView();
        }
    }
}
