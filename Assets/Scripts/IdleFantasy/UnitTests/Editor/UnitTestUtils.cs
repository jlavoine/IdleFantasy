using MyLibrary;
using System.Collections.Generic;
using NSubstitute;

namespace IdleFantasy.UnitTests {
    public static class UnitTestUtils {
        private static IdleFantasyOfflineBackend mOfflineBackend;

        public static void LoadOfflineData() {
            mOfflineBackend = new IdleFantasyOfflineBackend();
            BackendManager.Backend = mOfflineBackend;

            LoadConstants();
            LoadGenericData();
        }

        private static void LoadConstants() {
            Constants.Init( mOfflineBackend );
        }

        private static void LoadGenericData() {
            GenericDataLoader.Init( mOfflineBackend );
            GenericDataLoader.LoadDataOfClass<BuildingData>( GenericDataLoader.BUILDINGS );
            GenericDataLoader.LoadDataOfClass<UnitData>( GenericDataLoader.UNITS );
            GenericDataLoader.LoadDataOfClass<GuildData>( GenericDataLoader.GUILDS );
        }

        public static IPlayerData LoadMockPlayerData() {
            IPlayerData mockPlayerData = CreateMockPlayer();
            PlayerManager.Init( mockPlayerData );

            return mockPlayerData;
        }

        private static IPlayerData CreateMockPlayer() {
            IPlayerData mockPlayerData = NSubstitute.Substitute.For<IPlayerData>();

            SetPlayerGuilds( mockPlayerData );
            SetPlayerBuildings( mockPlayerData );

            return mockPlayerData;
        }

        private static void SetPlayerBuildings( IPlayerData i_mockPlayerData ) {
            List<Building> buildings = GetTestBuildingList();
            i_mockPlayerData.Buildings.Returns( buildings );
        }

        private static List<Building> GetTestBuildingList() {
            List<Building> buildings = new List<Building>();

            buildings.Add( new Building( new BuildingProgress() { ID = GenericDataLoader.TEST_BUILDING, Level = 1 }, new UnitProgress() { ID = GenericDataLoader.TEST_UNIT, Level = 1, Trainers = 1 } ) );
            buildings.Add( new Building( new BuildingProgress() { ID = GenericDataLoader.TEST_BUILDING_2, Level = 1 }, new UnitProgress() { ID = GenericDataLoader.TEST_UNIT_3, Level = 1, Trainers = 1 } ) );

            return buildings;
        }

        private static void SetPlayerGuilds( IPlayerData i_mockPlayerData ) {
            List<Guild> guilds = GetTestGuildList();
            i_mockPlayerData.Guilds.Returns( guilds );
        }

        private static List<Guild> GetTestGuildList() {
            GuildProgress guild_1 = new GuildProgress();
            guild_1.ID = "GUILD_1";
            guild_1.Level = 1;

            GuildProgress guild_2 = new GuildProgress();
            guild_2.ID = "GUILD_2";
            guild_2.Level = 1;

            List<Guild> guilds = new List<Guild>();
            guilds.Add( new Guild( guild_1 ) );
            guilds.Add( new Guild( guild_2 ) );

            return guilds;
        }
    }
}