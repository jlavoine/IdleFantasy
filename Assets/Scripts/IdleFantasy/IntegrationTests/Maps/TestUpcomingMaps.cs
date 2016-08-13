using System.Collections.Generic;
using MyLibrary;

namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestUpcomingMaps {
        public TestUpcomingMaps( MapData i_data, int i_level ) {
            int nextMapLevel = i_level + 1;

            CheckUpcomingMapSize( i_data.UpcomingMaps );
            CheckUpcomingMapNames( i_data.UpcomingMaps, nextMapLevel );
        }

        private void CheckUpcomingMapSize( List<MapName> i_upcomingMaps ) {
            int expectedSize = Constants.GetConstant<int>( ConstantKeys.UPCOMING_MAPS_COUNT );
            if ( expectedSize != i_upcomingMaps.Count ) {
                IntegrationTest.Fail( string.Format( "Upcoming maps size fail, got {0}, expecting {1}", i_upcomingMaps.Count, expectedSize ) );
            }
        }

        private void CheckUpcomingMapNames( List<MapName> i_upcomingMaps, int i_level ) {
            foreach ( MapName mapName in i_upcomingMaps ) {
                new TestMapNames( mapName, i_level );
            }
        }
    }
}
