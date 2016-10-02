using System.Collections.Generic;

namespace MyLibrary {
    public class QueuedCloudCall  {
        public string MethodName;
        public Dictionary<string, string> Params;
        public Callback<Dictionary<string, string>> SuccessCallback;

        public QueuedCloudCall( string i_methodName, Dictionary<string, string> i_params, Callback<Dictionary<string, string>> i_requestSuccessCallback ) {
            MethodName = i_methodName;
            Params = i_params;
            SuccessCallback = i_requestSuccessCallback;
        }
    }
}