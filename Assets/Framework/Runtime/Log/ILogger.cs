namespace GBG.Framework.Log
{
    public enum LogLevel
    {
        Trace = 0,
        Debug,
        Info,
        Warn,
        Error,
        Fatal,
        Off,
    }

    public interface ILogger
    {
        // NLog format: "${datetime}|${LEVEL}|${logger}|${message}"
        // Log4net format: "%timestamp [%thread] %-5level %logger - %message"

        void Log(LogLevel level, string tag, string message, object context);
        //[System.Diagnostics.Conditional("GBG_FRAMEWORK_LOG_TRACE")]
        void LogTrace(string message, object context = null, string tag = null);
        //[System.Diagnostics.Conditional("GBG_FRAMEWORK_LOG_DEBUG")]
        void LogDebug(string message, object context = null, string tag = null);
        void LogInfo(string message, object context = null, string tag = null);
        void LogWarn(string message, object context = null, string tag = null);
        //[System.Diagnostics.Conditional("GBG_FRAMEWORK_LOG_ASSERT")]
        void LogAssert(bool condition, string message, object context = null, string tag = null);
        void LogError(string message, object context = null, string tag = null);
        void LogFatal(string message, object context = null, string tag = null);
    }
}
