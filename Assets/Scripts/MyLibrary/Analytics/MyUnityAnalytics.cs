using System.Collections.Generic;
using UnityEngine.Analytics;

namespace MyLibrary {
    public class MyUnityAnalytics : IUnityAnalytics {
        public AnalyticsResult SendCustomEvent( string i_eventName, IDictionary<string, object> i_eventData ) {
            return Analytics.CustomEvent( i_eventName, i_eventData );
        }
    }
}