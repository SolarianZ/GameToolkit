using System;
using System.Diagnostics;
using System.Threading;

namespace GBG.GameToolkit
{
    public enum LogLevel
    {
        // ReSharper disable InconsistentNaming
        // Use uppercase so that ToString gets uppercase
        TRACE = -2,
        DEBUG = -1,
        INFO = 0,
        WARN = 1,
        ERROR = 2,
        FATAL = 3,
        OFF = 127,
        // ReSharper restore InconsistentNaming
    }

    public delegate void AssertionFailureHandler(string message);

    public delegate void LogHandler(LogLevel level, string tag, string message, object context);

    public static class Debugger
    {
        #region Settings

        public static LogLevel LogFilterLevel { get; set; }
#if UNITY_EDITOR
            = LogLevel.DEBUG;
#else
            = LogLevel.INFO;
#endif
        public static string TimestampFormat { get; set; } = "yyyy-MM-dd HH:mm:ss:fff";
        public static AssertionFailureHandler AssertionFailureHandler { get; set; }
        public static LogHandler Logger { get; set; }

        #endregion


        public static string CombineLogContent(LogLevel level, string tag, string message, object context)
        {
            // timestamp thread LEVEL [tag] context | message
            var timestamp = DateTime.UtcNow.ToString(TimestampFormat);
            var threadId = Thread.CurrentThread.ManagedThreadId.ToString();
            var levelStr = level.ToString();
            var content = $"{timestamp} {threadId} {levelStr} [{(string.IsNullOrEmpty(tag) ? "-" : tag)}] {context?.ToString() ?? "-"} | {message}";
            return content;
        }

        public static void Log(LogLevel level, string tag, string message, object context)
        {
            CheckLogger();
            Logger.Invoke(level, tag, message, context);
        }

        [Conditional("GBG_FRAMEWORK_LOG_TRACE")]
        public static void LogTrace(string message, object context = null, string tag = null)
        {
            CheckLogger();
            Logger.Invoke(LogLevel.TRACE, tag, message, context);
        }

        [Conditional("GBG_FRAMEWORK_LOG_DEBUG")]
        public static void LogDebug(string message, object context = null, string tag = null)
        {
            CheckLogger();
            Logger.Invoke(LogLevel.DEBUG, tag, message, context);
        }

        public static void LogInfo(string message, object context = null, string tag = null)
        {
            CheckLogger();
            Logger.Invoke(LogLevel.INFO, tag, message, context);
        }

        public static void LogWarn(string message, object context = null, string tag = null)
        {
            CheckLogger();
            Logger.Invoke(LogLevel.WARN, tag, message, context);
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        [Conditional("GBG_FRAMEWORK_LOG_ASSERT")]
        public static void LogAssertError(bool condition, string message, object context = null, string tag = null)
        {
            if (condition) return;

            CheckLogger();
            Logger.Invoke(LogLevel.ERROR, tag, message, context);
        }

        public static void LogError(string message, object context = null, string tag = null)
        {
            CheckLogger();
            Logger.Invoke(LogLevel.ERROR, tag, message, context);
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        [Conditional("GBG_FRAMEWORK_LOG_ASSERT")]
        public static void LogAssertFatal(bool condition, string message, object context = null, string tag = null)
        {
            if (condition) return;

            CheckLogger();
            Logger.Invoke(LogLevel.FATAL, tag, message, context);
        }

        public static void LogFatal(string message, object context = null, string tag = null)
        {
            CheckLogger();
            Logger.Invoke(LogLevel.FATAL, tag, message, context);
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        [Conditional("GBG_AI_DEBUG")]
        public static void Assert(bool condition, string message)
        {
            if (condition) return;

            CheckAsserter();
            AssertionFailureHandler?.Invoke(message);
        }

        private static void CheckLogger()
        {
            if (Logger == null)
            {
                throw new NullReferenceException($"System.NullReferenceException: Please assign the '{nameof(Logger)}' before logging.");
            }
        }

        private static void CheckAsserter()
        {
            if (AssertionFailureHandler == null)
            {
                throw new NullReferenceException($"System.NullReferenceException: Please assign the '{nameof(AssertionFailureHandler)}' before asserting.");
            }
        }
    }
}