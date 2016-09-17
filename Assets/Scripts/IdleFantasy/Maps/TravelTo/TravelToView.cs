using MyLibrary;
using UnityEngine;

namespace IdleFantasy {
    public class TravelToView : GroupView {
        public GameObject TravelOptionPrefab;
        public GameObject TravelOptionContent;

        public GameObject TravelingPopup;

        public void Init( TravelTo i_travelTo ) {
            SetModel( i_travelTo.ViewModel );
            SubscribeToMessages();
            CreateAndInitTravelOptions( i_travelTo );
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

        private void CreateAndInitTravelOptions( TravelTo i_travelTo ) {
            foreach ( TravelOption option in i_travelTo.TravelOptions ) {
                GameObject optionObject = gameObject.InstantiateUI( TravelOptionPrefab, TravelOptionContent );
                TravelOptionView optionView = optionObject.GetComponent<TravelOptionView>();
                optionView.Init( option );
            }
        }

        private void OnTravelToSelected( int i_travelOptionIndex ) {
            CreateTravelingPopup();
            CloseView();            
        }

        private void CreateTravelingPopup() {
            GameObject mainCanvas = GameObject.Find( "MainCanvas" );
            mainCanvas.InstantiateUI( TravelingPopup );
        }
    }
}
