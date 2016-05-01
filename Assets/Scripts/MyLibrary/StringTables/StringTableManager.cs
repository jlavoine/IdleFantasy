using UnityEngine;
using System.Collections;

namespace MyLibrary {
    public static class StringTableManager {
        private static StringTable mStringTable;

        private static IMessageService mMessenger;

        public static void Init( string i_langauge, IBackend i_backend, IMessageService i_messenger ) {
            mMessenger = i_messenger;

            mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Initing string table for " + i_langauge, "" );

            string tableKey = "StringTable_" + i_langauge;
            i_backend.GetTitleData( tableKey, CreateTableFromJSON );
        }

        private static void CreateTableFromJSON( string i_tableJSON ) {
            mStringTable = new StringTable( i_tableJSON, mMessenger );
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