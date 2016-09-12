
namespace MyLibrary {
    public interface IAnalyticsTimer {

        void Start();
        void StopTimer();
        void StopAndSendAnalytic();

        void StepComplete( string i_stepName );
    }
}