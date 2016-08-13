
namespace IdleFantasy.PlayFab.IntegrationTests {
    public class TestMapNames  {

        public TestMapNames( MapName i_mapName, int i_level ) {
            CheckMapNames( i_mapName, i_level );
        }

        // how to improve this? Make the parts of the map name into an array?
        private void CheckMapNames( MapName i_mapName, int i_level ) {
            if ( i_mapName.Prefix.PieceType != MapPieceTypes.Prefix ) {
                IntegrationTest.Fail( "Map name test failed: Prefix piece type was " + i_mapName.Prefix.PieceType + " and not " + MapPieceTypes.Prefix );
            }

            if ( !i_mapName.Prefix.LevelRestriction.DoesPass( i_level ) ) {
                IntegrationTest.Fail( "Map name test failed: Prefix level not valid" );
            }

            if ( i_mapName.Terrain.PieceType != MapPieceTypes.Terrain ) {
                IntegrationTest.Fail( "Map name test failed: Terrain piece type was " + i_mapName.Terrain.PieceType + " and not " + MapPieceTypes.Terrain );
            }

            if ( !i_mapName.Terrain.LevelRestriction.DoesPass( i_level ) ) {
                IntegrationTest.Fail( "Map name test failed: Terrain level not valid" );
            }

            if ( i_mapName.Suffix.PieceType != MapPieceTypes.Suffix ) {
                IntegrationTest.Fail( "Map name test failed: Suffix piece type was " + i_mapName.Suffix.PieceType + " and not " + MapPieceTypes.Suffix );
            }

            if ( !i_mapName.Suffix.LevelRestriction.DoesPass( i_level ) ) {
                IntegrationTest.Fail( "Map name test failed: Suffix level not valid" );
            }
        }
    }
}
