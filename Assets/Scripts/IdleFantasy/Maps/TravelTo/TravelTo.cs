using MyLibrary;
using System.Collections.Generic;

namespace IdleFantasy {
    public class TravelTo : GenericViewModel {
        public const string TITLE_PROPERTY = "Title";
        public const string NEXT_CONTINENT_PROPERTY = "NextContinentTitle";
        public const string NEXT_CONTINENT_VISIBLE_PROPERTY = "NextContinentVisible";

        private List<TravelOption> mTravelOptions = new List<TravelOption>();
        public List<TravelOption> TravelOptions { get { return mTravelOptions; } }

        public TravelTo( IMapData i_map ) {
            SetStringTableProperties();
            SetNextContinentButtonVisibility( i_map.GetLevel() );
            CreateTravelOptions( i_map.GetUpcomingMaps() );
        }

        private void SetStringTableProperties() {
            ViewModel.SetProperty( TITLE_PROPERTY, StringTableManager.Get( StringKeys.TRAVEL_TO_TITLE ) );
            ViewModel.SetProperty( NEXT_CONTINENT_PROPERTY, StringTableManager.Get( StringKeys.NEXT_CONTINENT_TITLE ) );
        }

        private void SetNextContinentButtonVisibility( int i_level ) {
            int levelRequired = Constants.GetConstant<int>( ConstantKeys.NEXT_CONTINENT_LEVEL_MIN );
            bool isVisible = i_level >= levelRequired;
            ViewModel.SetProperty( NEXT_CONTINENT_VISIBLE_PROPERTY, isVisible ? 1f : 0f );
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
