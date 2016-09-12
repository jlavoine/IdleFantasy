using UnityEngine.Analytics;
using System.Collections.Generic;

namespace MyLibrary {
    public interface IUnityAnalytics  {
        AnalyticsResult SendCustomEvent( string i_eventName, IDictionary<string, object> i_eventData );
    }
}
