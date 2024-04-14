using System;
using System.Threading;

namespace GBG.GameToolkit.Log
{
    [Obsolete("Use GBG.GameToolkit.Debugger.", true)]
    public interface ILogger
    {
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

        // timestamp thread LEVEL tag context message
        public static string CombineLogContent(string timestampFormat, LogLevel level, string tag, string message, object context)
        {
            var timestamp = DateTime.UtcNow.ToString(timestampFormat);
            var threadId = Thread.CurrentThread.ManagedThreadId.ToString();
            var levelStr = level.ToString();
            var content = $"{timestamp} {threadId} {levelStr} [{(string.IsNullOrEmpty(tag) ? "-" : tag)}] {(context?.ToString() ?? "-")} {message}";
            return content;
        }
    }
}