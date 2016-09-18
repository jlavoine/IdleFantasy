
namespace MyLibrary {
    public static class LibraryAnalyticEvents {
        // all analytics should send a message using this event
        public const string SEND_ANALYTIC_EVENT = "SendAnalyticEvent";

        #region Analytic Events
        public const string CLIENT_LOST_SYNC = "ClientOutOfSync";
        public const string LOGIN_TIME = "LoginTime";
        #endregion

        #region Event Data Keys
        public const string REASON = "Reason";
        public const string TOTAL_TIME = "TotalTime";
        #endregion

        #region Login Time Event Keys 
        public const string AUTH_TIME = "Authentication";
        public const string CLOUD_SETUP_TIME = "CloudSetup";
        public const string ON_LOGIN_TIME = "OnLogin";
        public const string TITLE_DATA_TIME = "GetTitleData";
        public const string INIT_PLAYER_TIME = "InitPlayer";
        #endregion
    }
}
