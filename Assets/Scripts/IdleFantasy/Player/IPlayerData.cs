using MyLibrary;
using System.Collections.Generic;

namespace IdleFantasy {
    public interface IPlayerData {
        void Dispose();

        ViewModel GetViewModel();

        int Gold {
            get;
        }
        //object GetData( string i_key );

        List<Guild> Guilds { get; }
        List<Building> Buildings { get; }
        ITrainerManager TrainerManager { get; }
        Dictionary<string, MapData> Maps { get; }
        Dictionary<string, WorldMissionProgress> MissionProgress { get; }
        IGameMetrics GameMetrics { get; }
        Dictionary<string, UnitProgress> UnitProgress { get; }

        UnitUnlockPlanData UnitUnlockPlan { get; }

        IWorldMissionProgress GetMissionProgressForWorld( string i_world );
        IRepeatableQuestProgress GetRepeatableQuestForWorld( string i_world );
        IMapData GetMapDataForWorld( string i_world );

        void PlayerTraveledToNewArea( Dictionary<string, string> i_travelData );
        void PlayerResetWorld( Dictionary<string, string> i_newMapData );
    }
}
