using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyLibrary {
    public class StringTable {
        private Dictionary<string, StringTableEntry> mStringTable = new Dictionary<string, StringTableEntry>();

        private IMessageService mMessenger;

        public StringTable( string i_tableJSON, IMessageService i_messenger ) {
            mMessenger = i_messenger;
            mStringTable = JsonConvert.DeserializeObject<Dictionary<string, StringTableEntry>>( i_tableJSON );
        }

        public string Get( string i_strKey ) {
            string strResult = "???";

            if ( mStringTable.ContainsKey( i_strKey ) == false ) {
                mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Warn, "String table had no key for " + i_strKey, "" );
            }
            else {
                strResult = mStringTable[i_strKey].Value;
            }

            return strResult;
        }
    }
}
