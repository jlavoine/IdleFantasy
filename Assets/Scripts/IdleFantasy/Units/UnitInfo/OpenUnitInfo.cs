using UnityEngine;

namespace IdleFantasy {
    public class OpenUnitInfo : MonoBehaviour {
        public GameObject UnitInfoPrefab;

        private int mUnitIndex = 0;

        public void SetUnitIndex( int i_index ) {
            mUnitIndex = i_index;
        }

        public void OnClick() {
            OpenUnitInfoWindow();
        }

        private void OpenUnitInfoWindow() {
            GameObject window = gameObject.InstantiateUI( UnitInfoPrefab );

            UnitInfoPM infoPM = new UnitInfoPM( PlayerManager.Data.AllUnits, mUnitIndex, StatCalculator.Instance );
            UnitInfoView infoView = window.GetComponent<UnitInfoView>();
            infoView.Init( infoPM );
        }
    }
}