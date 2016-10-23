using MyLibrary;
using UnityEngine;

namespace IdleFantasy {
    public class UnitInfoView : GroupView {
        public GameObject UnitStatEntryPrefab;
        public GameObject UnitStatsContentArea;

        private UnitInfoPM mUnitInfoPM;
                
        public void Init( UnitInfoPM i_infoPM ) {
            mUnitInfoPM = i_infoPM;
            SetModel( mUnitInfoPM.ViewModel );

            RefreshUnitStatDisplay();
        }

        public void OnNextUnitButtonClicked() {
            mUnitInfoPM.GoToNextUnit();
            RefreshUnitStatDisplay();
        }

        public void OnPreviousUnitButtonClicked() {
            mUnitInfoPM.GoToPreviousUnit();
            RefreshUnitStatDisplay();
        }

        private void RefreshUnitStatDisplay() {
            UnitStatsContentArea.DestroyChildren();
            foreach ( UnitStatPM statPM in mUnitInfoPM.StatPMs ) {
                GameObject statViewObject = gameObject.InstantiateUI( UnitStatEntryPrefab, UnitStatsContentArea );
                UnitStatView statView = statViewObject.GetComponent<UnitStatView>();
                statView.Init( statPM );
            }
        }
    }
}
