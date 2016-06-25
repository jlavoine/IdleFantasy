
namespace IdleFantasy {
    public static class PlayerManager {

        public static IPlayerData Data;

        public static void Init( IPlayerData i_data ) {
            Data = i_data;
        }
    }
}
