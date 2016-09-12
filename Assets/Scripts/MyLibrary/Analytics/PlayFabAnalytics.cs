using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

namespace MyLibrary{
    public class PlayFabAnalytics {

       public PlayFabAnalytics() {
           // MyMessenger.AddListener<string, long>( AnalyticsTimer.TIMER_EVENT, OnTimerAnalytic );
        }

        public void Dispose() {
            //MyMessenger.RemoveListener<string, long>( AnalyticsTimer.TIMER_EVENT, OnTimerAnalytic );
        }

        // currently don't have access to send custom analytics with playfab...
        public void OnTimerAnalytic( string i_analyticName, long i_elapsedMillis ) {
            /*mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Attempt log event: " + i_analyticName, PlayFabBackend.PLAYFAB );

            Dictionary<string, object> body = new Dictionary<string, object>();
            body.Add( i_analyticName, i_elapsedMillis );

            LogEventRequest request = new LogEventRequest() {
                EventName = AnalyticsTimer.TIMER_EVENT,
                Body = body,
                ProfileSetEvent = false
            };

            PlayFabClientAPI.LogEvent( request, ( result ) => {
                mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Info, "Log event success: " + i_analyticName, PlayFabBackend.PLAYFAB );
            },
            ( error ) => {
                mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Warn, "Log event failure: " + i_analyticName, PlayFabBackend.PLAYFAB );
                mMessenger.Send<LogTypes, string, string>( MyLogger.LOG_EVENT, LogTypes.Warn, "Analytics error: " + error.ErrorMessage, PlayFabBackend.PLAYFAB );
            } );*/
        }
    }
}