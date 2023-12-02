#if UNITY_2022_1_OR_NEWER || GBG_FRAMEWORK_ENABLE_UNITY_APIS
using System;
using System.Threading;
using GBG.GameToolkit.Log;
using UDebug = UnityEngine.Debug;
using UObject = UnityEngine.Object;

namespace GBG.GameToolkit.Unity
{
    public class UnityLogger : ILogger
    {
        public LogLevel LogFilterLevel { get; set; }
#if UNITY_EDITOR
        = LogLevel.DEBUG;
#else
        = LogLevel.INFO;
#endif
        public string TimestampFormat { get; set; } = "yyyy-MM-dd HH:mm:ss:fff";

        // timestamp thread LEVEL tag context message

        public static string CombineLogContent(string timestampFormat, LogLevel level, string tag, string message, object context)
        {
            var timestamp = DateTime.UtcNow.ToString(timestampFormat);
            var threadId = Thread.CurrentThread.ManagedThreadId.ToString();
            var levelStr = level.ToString();
            var content = $"{timestamp} {threadId} {levelStr} [{(string.IsNullOrEmpty(tag) ? "-" : tag)}] {(context?.ToString() ?? "-")} {message}";
            return content;
        }

        public void Log(LogLevel level, string tag, string message, object context)
        {
            if (level < LogFilterLevel) return;

            var content = CombineLogContent(TimestampFormat, level, tag, message, context);
            switch (level)
            {
                case LogLevel.TRACE:
                case LogLevel.DEBUG:
                case LogLevel.INFO:
                    UDebug.Log(content, context as UObject);
                    break;
                case LogLevel.WARN:
                    UDebug.LogWarning(content, context as UObject);
                    break;
                case LogLevel.ERROR:
                case LogLevel.FATAL:
                    UDebug.LogError(content, context as UObject);
                    break;
                case LogLevel.OFF:
                default:
                    UDebug.LogError($"Invalid log level: {level}. Log content: {content}", context as UObject);
                    break;
            }
        }

        //[System.Diagnostics.Conditional("GBG_FRAMEWORK_LOG_TRACE")]
        public void LogTrace(string message, object context = null, string tag = null)
        {
            Log(LogLevel.TRACE, tag, message, context);
        }

        //[System.Diagnostics.Conditional("GBG_FRAMEWORK_LOG_DEBUG")]
        public void LogDebug(string message, object context = null, string tag = null)
        {
            Log(LogLevel.DEBUG, tag, message, context);
        }

        public void LogInfo(string message, object context = null, string tag = null)
        {
            Log(LogLevel.INFO, tag, message, context);
        }

        public void LogWarn(string message, object context = null, string tag = null)
        {
            Log(LogLevel.WARN, tag, message, context);
        }

        //[System.Diagnostics.Conditional("GBG_FRAMEWORK_LOG_ASSERT")]
        public void LogAssertError(bool condition, string message, object context = null, string tag = null)
        {
            if (condition) return;

            Log(LogLevel.ERROR, tag, message, context);
        }

        public void LogError(string message, object context = null, string tag = null)
        {
            Log(LogLevel.ERROR, tag, message, context);
        }

        //[System.Diagnostics.Conditional("GBG_FRAMEWORK_LOG_ASSERT")]
        public void LogAssertFatal(bool condition, string message, object context = null, string tag = null)
        {
            if (condition) return;

            Log(LogLevel.FATAL, tag, message, context);
        }

        public void LogFatal(string message, object context = null, string tag = null)
        {
            Log(LogLevel.FATAL, tag, message, context);
        }
    }
}
#endif