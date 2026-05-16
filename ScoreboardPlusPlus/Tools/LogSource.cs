using BepInEx.Logging;

namespace ScoreboardPlusPlus.Tools
{
    public class LogSource
    {
        private static ManualLogSource log;

        private static ManualLogSource Instance => log ??= Logger.CreateLogSource(Constants.Name);

        public static void LogInfo(string message) => Instance.LogInfo(message);
        public static void LogMessage(string message) => Instance.LogMessage(message);
        public static void LogWarning(string message) => Instance.LogWarning(message);
        public static void LogError(string message) => Instance.LogError(message);
    }
}