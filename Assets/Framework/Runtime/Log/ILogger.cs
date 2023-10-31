namespace GBG.Framework.Log
{
    public enum LogType
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal,
    }

    public interface ILogger
    {
        void Log(LogType type, string tag, string message, object context);
        //[System.Diagnostics.Conditional("GBG_FRAMEWORK_LOG_DEBUG")]
        void LogDebug(string message, object context = null, string tag = null);
        void LogInfo(string message, object context = null, string tag = null);
        void LogWarning(string message, object context = null, string tag = null);
        void LogError(string message, object context = null, string tag = null);
        void LogFatal(string message, object context = null, string tag = null);
    }
}
