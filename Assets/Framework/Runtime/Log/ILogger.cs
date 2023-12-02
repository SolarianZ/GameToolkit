namespace GBG.GameToolkit.Log
{
    public enum LogLevel
    {
        TRACE = 0,
        DEBUG,
        INFO,
        WARN,
        ERROR,
        FATAL,
        OFF,
    }

    public interface ILogger
    {
        // timestamp thread LEVEL tag context message

        LogLevel LogFilterLevel { get; set; }
        string TimestampFormat { get; set; }

        void Log(LogLevel level, string tag, string message, object context);
        //[System.Diagnostics.Conditional("GBG_FRAMEWORK_LOG_TRACE")]
        void LogTrace(string message, object context = null, string tag = null);
        //[System.Diagnostics.Conditional("GBG_FRAMEWORK_LOG_DEBUG")]
        void LogDebug(string message, object context = null, string tag = null);
        void LogInfo(string message, object context = null, string tag = null);
        void LogWarn(string message, object context = null, string tag = null);
        //[System.Diagnostics.Conditional("GBG_FRAMEWORK_LOG_ASSERT")]
        void LogAssertError(bool condition, string message, object context = null, string tag = null);
        void LogError(string message, object context = null, string tag = null);
        //[System.Diagnostics.Conditional("GBG_FRAMEWORK_LOG_ASSERT")]
        void LogAssertFatal(bool condition, string message, object context = null, string tag = null);
        void LogFatal(string message, object context = null, string tag = null);
    }
}
