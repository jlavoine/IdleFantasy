using System.Collections.Generic;

namespace IdleFantasy {
    public interface IMapData {
        List<MapName> GetUpcomingMaps();
        int GetLevel();
    }
}