﻿using MyLibrary;

namespace IdleFantasy {
    public class MapAreaView : GroupView {
        public const string AREA_CLICKED_MESSAGE = "AreaSelected";

        private MapArea mArea;

        public void Init( MapArea i_area ) {
            mArea = i_area;

            SetModel( i_area.ViewModel );
        }

        public void OnMapAreaClicked() {
            MyMessenger.Send( AREA_CLICKED_MESSAGE );
            MissionInterfaceManager.Instance.CreateUI( new Mission( mArea.Data.Mission ) );
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            mArea.Dispose();
        }
    }
}
