using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

namespace MyLibrary {
    public class PlayFabAnalytics {

        private IMessageService mMessenger;
        private ILogService mLogger;

       public PlayFabAnalytics( IMessageService i_messenger, ILogService i_logger ) {
            mMessenger = i_messenger;
            mLogger = i_logger;

            mMessenger.AddListener<string, long>( AnalyticsTimer.TIMER_EVENT, OnTimerAnalytic );
        }

        public void Dispose() {
            mMessenger.RemoveListener<string, long>( AnalyticsTimer.TIMER_EVENT, OnTimerAnalytic );
        }

        private void OnTimerAnalytic( string i_analyticName, long i_elapsedMillis ) {
            mLogger.Log( LogTypes.Info, "Attempt log event: " + i_analyticName, PlayFabBackend.PLAYFAB );

            Dictionary<string, object> body = new Dictionary<string, object>();
            body.Add( i_analyticName, i_elapsedMillis );

            LogEventRequest request = new LogEventRequest() {
                EventName = AnalyticsTimer.TIMER_EVENT,
                Body = body,
                ProfileSetEvent = false
            };

            PlayFabClientAPI.LogEvent( request, ( result ) => {
                mLogger.Log( LogTypes.Info, "Log event success: " + i_analyticName, PlayFabBackend.PLAYFAB );
            },
            ( error ) => {
                mLogger.Log( LogTypes.Warn, "Log event failure: " + i_analyticName, PlayFabBackend.PLAYFAB );
            } );
        }
    }
}