using MyLibrary;
using UnityEngine;

namespace IdleFantasy {
    public class TravelToView : GroupView {
        public GameObject TravelOptionPrefab;
        public GameObject TravelOptionContent;

        public void Init( TravelTo i_travelTo ) {
            SetModel( i_travelTo.ViewModel );

            CreateAndInitTravelOptions( i_travelTo );
        }

        private void CreateAndInitTravelOptions( TravelTo i_travelTo ) {
            foreach ( TravelOption option in i_travelTo.TravelOptions ) {
                GameObject optionObject = gameObject.InstantiateUI( TravelOptionPrefab, TravelOptionContent );
                TravelOptionView optionView = optionObject.GetComponent<TravelOptionView>();
                optionView.Init( option );
            }
        }
    }
}
