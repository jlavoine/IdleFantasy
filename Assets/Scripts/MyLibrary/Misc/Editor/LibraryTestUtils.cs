using NSubstitute;

namespace MyLibrary {
    public static class LibraryTestUtils {
        public static ILogService ReplaceLogger() {
            ILogService logger = Substitute.For<ILogService>();
            EasyLogger.Instance = logger;
            return logger;
        }

        public static void ResetLogger() {
            EasyLogger.Instance = null;
        }
    }
}
