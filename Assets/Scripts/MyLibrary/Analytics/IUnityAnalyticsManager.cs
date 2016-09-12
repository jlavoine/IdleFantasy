using System.Collections.Generic;

namespace MyLibrary {
    public interface IUnityAnalyticsManager {
        void Dispose();

        void SendCustomUnityAnalytic( string i_eventName, IDictionary<string, object> i_eventData );
    }
}
