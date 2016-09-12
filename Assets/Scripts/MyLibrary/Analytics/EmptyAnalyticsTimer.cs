
namespace MyLibrary {
    public class EmptyAnalyticsTimer : IAnalyticsTimer {
        public void Start() {}
        public void StepComplete( string i_stepName ) {}    
        public void StopTimer() {}
        public void StopAndSendAnalytic() {}
    }
}