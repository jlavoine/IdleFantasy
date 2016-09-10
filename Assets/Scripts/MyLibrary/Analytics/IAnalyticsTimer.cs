
namespace MyLibrary {
    public interface IAnalyticsTimer {

        void Start();
        void Stop();
        void StopAndSendAnalytic();

        void StepComplete( string i_stepName );
    }
}