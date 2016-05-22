
namespace MyLibrary.PlayFab {
    public static class PlayFabUtils {

        /// <summary>
        /// Strings returned from the PlayFab cloud are a little difficult for Newtonsoft's JSON
        /// deserialization. This method will clean it up for proper deserialization.
        /// </summary>
        public static string CleanStringForJsonDeserialization( this string i_string ) {
            i_string = i_string.Replace( "\\\"", "\"" );

            i_string = i_string.Replace( "\"[", "[" );
            i_string = i_string.Replace( "]\"", "]" );

            i_string = i_string.Replace( "\"{", "{" );
            i_string = i_string.Replace( "}\"", "}" );            

            return i_string;
        }
    }
}
