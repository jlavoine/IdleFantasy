using System.Collections.Generic;
using UnityEngine.Analytics;

namespace MyLibrary {
    public class UnityAnalyticsManager {
        private const string LOG_TYPE = "Analytics";

        public UnityAnalyticsManager() {
            MyMessenger.AddListener<string, IDictionary<string, object>>( LibraryAnalyticEvents.SEND_ANALYTIC_EVENT, OnAnalyticEvent );
        }

        public void Dispose() {
            MyMessenger.RemoveListener<string, IDictionary<string, object>>( LibraryAnalyticEvents.SEND_ANALYTIC_EVENT, OnAnalyticEvent );
        }

        private void OnAnalyticEvent( string i_eventName, IDictionary<string, object> i_eventData ) {
            SendLogEvent( i_eventName, i_eventData );
            SendCustomUnityAnalytic( i_eventName, i_eventData );
        }

        private void SendCustomUnityAnalytic( string i_eventName, IDictionary<string, object> i_eventData ) {
            AnalyticsResult result = Analytics.CustomEvent( i_eventName, i_eventData );

            if ( result != AnalyticsResult.Ok ) {
                string log = "Failed to send analytic " + i_eventName + " with reason: " + result.ToString();
                MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Warn, log, LOG_TYPE );
            }
        }

        private void SendLogEvent( string i_eventName, IDictionary<string, object> i_eventData ) {
            string log = "Attempting to send analtyic " + i_eventName + " with event data: \n";
            foreach ( KeyValuePair<string, object> param in i_eventData ) {
                log += param.Key + ": " + param.Value.ToString() + "\n";
            }

            MyMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, log, LOG_TYPE );
        }
    }
}
