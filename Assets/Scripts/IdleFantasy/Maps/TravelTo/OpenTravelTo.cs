using UnityEngine;

namespace IdleFantasy {
    public class OpenTravelTo : MonoBehaviour {
        public GameObject TravelToPopupPrefab;

        public void OnClick() {
            CreateTravelToView();
        }

        private void CreateTravelToView() {
            TravelTo travelTo = new TravelTo( PlayerManager.Data.GetMapDataForWorld( BackendConstants.WORLD_BASE ) );

            GameObject mainCanvas = GameObject.FindGameObjectWithTag( "MainCanvas" );
            GameObject travelToUI = gameObject.InstantiateUI( TravelToPopupPrefab, mainCanvas );
            TravelToView view = travelToUI.GetComponent<TravelToView>();
            view.Init( travelTo );
        }
    }
}
