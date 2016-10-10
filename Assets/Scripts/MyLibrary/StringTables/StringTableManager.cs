using UnityEngine;
using System.Collections;

namespace MyLibrary {
    public static class StringTableManager {
        private static StringTable mStringTable;

        public static void Init( string i_langauge, IBasicBackend i_backend ) {
            MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Initing string table for " + i_langauge, "" );

            string tableKey = "SimpleStringTable_" + i_langauge;
            i_backend.GetTitleData( tableKey, CreateTableFromJSON );
        }

        private static void CreateTableFromJSON( string i_tableJSON ) {
            mStringTable = new StringTable( i_tableJSON );
        }

        public static string Get( string i_key ) {
            if ( mStringTable != null ) {
                return mStringTable.Get( i_key );
            } 
            else {
                return "No string table";
            }
        }
    }
}