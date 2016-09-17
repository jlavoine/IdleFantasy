using MyLibrary;
using System.Collections.Generic;

namespace IdleFantasy {
    public class TravelTo : GenericViewModel {
        public const string TITLE_PROPERTY = "Title";

        private List<TravelOption> mTravelOptions = new List<TravelOption>();
        public List<TravelOption> TravelOptions { get { return mTravelOptions; } }

        public TravelTo( List<MapName> i_areas ) {
            SetTitleProperty();
            CreateTravelOptions( i_areas );
        }

        private void SetTitleProperty() {
            ViewModel.SetProperty( TITLE_PROPERTY, StringTableManager.Get( StringKeys.TRAVEL_TO_TITLE ) ); 
        }

        private void CreateTravelOptions( List<MapName> i_areas ) {
            IWorldMissionProgress missionProgress = PlayerManager.Data.GetMissionProgressForWorld( BackendConstants.WORLD_BASE );
            foreach ( MapName mapName in i_areas ) {
                CreateTravelOption( mapName, missionProgress );
            }
        }

        private void CreateTravelOption( MapName i_mapName, IWorldMissionProgress i_missionProgress ) {
            int index = mTravelOptions.Count;

            mTravelOptions.Add( new TravelOption( i_mapName, index, i_missionProgress ) );
        }
    }
}
