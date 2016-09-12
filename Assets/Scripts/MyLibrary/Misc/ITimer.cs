
namespace MyLibrary {
    public interface ITimer {
        void Start();
        void Stop();
        void Restart();

        long GetElapsedMilliseconds();   
    }
}