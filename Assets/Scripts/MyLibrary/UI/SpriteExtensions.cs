using UnityEngine;

namespace MyLibrary {
    public static class SpriteExtensions {

       public static Sprite GetSpriteFromResource( string i_key ) {
            Sprite spriteFromResource = Resources.Load<Sprite>( i_key );
            if ( spriteFromResource == null ) {
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Warn, "Loaded null sprite from resources for key: " + i_key, "" );
            }

            return spriteFromResource;
        }
    }
}
